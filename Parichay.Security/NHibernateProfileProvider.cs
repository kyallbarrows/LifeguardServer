﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Profile;
using Parichay.Security.Entity;
using Parichay.Security.Helper;
using Parichay.Security.Util;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.Configuration;
using NHibernate;
using System.Globalization;
using System.Collections;
using System.Web;
using System.IO;

namespace Parichay.Security
{
    class NHibernateProfileProvider : ProfileProvider
    {
        private Application application = new Application();

        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from Web.config.
            if (null == config)
            {
                throw (new ArgumentNullException("config"));
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "NHibernateProfileProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NHibernate Profile Provider");
            }

            base.Initialize(name, config);

            application =
                NHibernateProviderEntityHelper.CreateOrLoadApplication(
                    ConfigurationUtil.GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath));
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            int result = 0;
            List<Profile> profiles = (List<Profile>)NHibernateHelper.FindByNamedQuery("Profile.GetByLastUpdate", userInactiveSinceDate, NHibernateUtil.DateTime);
            result = profiles.Count;
            foreach (var prof in profiles)
            {
                NHibernateHelper.Delete(prof);
            }
            return result;
        }

        public override int DeleteProfiles(string[] usernames)
        {
            int result = 0;

            foreach (var uname in usernames)
            {
                User user = NHibernateProviderEntityHelper.GetUser(uname);
                Profile profile = (Profile)NHibernateHelper.FindByNamedQuery("Profile.ByUserId", user.Id, NHibernateUtil.Int32)[0];
                NHibernateHelper.Delete(profile);
                result++;
            }
            return result;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            foreach (var prof in profiles)
            {
                NHibernateHelper.Delete(prof);
            }
            return profiles.Count;
        }

        bool isAnonymous()
        {
            HttpContext current = HttpContext.Current;

            if (current != null)
            {
                if (current.Request.IsAuthenticated)
                {
                    return false;
                }
            }
            return true;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            ProfileInfoCollection infos = new ProfileInfoCollection();
            try
            {
                User u = NHibernateProviderEntityHelper.GetUser(usernameToMatch);
                Profile prof = NHibernateProviderEntityHelper.GetProfile(usernameToMatch);

                infos.Add(new ProfileInfo(u.Name, this.isAnonymous(), u.LastActivityDate, prof.LastActivityDate, prof.PropertyNames.Length + prof.PropertyValuesBinary.Length + prof.PropertyValuesString.Length));
                totalRecords = 1;

            }
            catch (Exception ex)
            {
                throw ExceptionUtil.NewProviderException(this, "FindInactiveProfilesByUserName", ex);
            }
            return infos;
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            return FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, DateTime.Now, 0, 0, out totalRecords);
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            ProfileInfoCollection infos = new ProfileInfoCollection();
            IList profiles = NHibernateHelper.FindPageByNamedQuery("Profile.GetByLastUpdate", userInactiveSinceDate, NHibernateUtil.DateTime, pageIndex, pageSize);
            totalRecords = profiles.Count;
            foreach (Profile prof in profiles)
            {
                User u = NHibernateProviderEntityHelper.GetUser(prof.UserId);

                infos.Add(new ProfileInfo(u.Name, this.isAnonymous(), u.LastActivityDate, prof.LastActivityDate, prof.PropertyNames.Length + prof.PropertyValuesBinary.Length + prof.PropertyValuesString.Length));
            }
            return infos;
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            ProfileInfoCollection infos = new ProfileInfoCollection();
            IList profiles = NHibernateHelper.FindPageByNamedQuery("Profile.GetAllProfiles", pageIndex, pageSize);
            totalRecords = profiles.Count;
            foreach (Profile prof in profiles)
            {
                User u = NHibernateProviderEntityHelper.GetUser(prof.UserId);

                infos.Add(new ProfileInfo(u.Name, this.isAnonymous(), u.LastActivityDate, prof.LastActivityDate, prof.PropertyNames.Length + prof.PropertyValuesBinary.Length + prof.PropertyValuesString.Length));
            }
            return infos;
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            IList profiles = NHibernateHelper.FindByNamedQuery("Profile.GetByLastUpdate", userInactiveSinceDate, NHibernateUtil.DateTime);
            return profiles.Count;
        }

        public override string ApplicationName
        {
            get { return application.Name; }
            set { application.Name = value; }
        }

        private void ParseDataFromDB(string[] names, string values, byte[] buf, SettingsPropertyValueCollection properties)
        {
            if (((names != null) && (values != null)) && ((buf != null) && (properties != null)))
            {
                try
                {
                    for (int i = 0; i < (names.Length / 4); i++)
                    {
                        string str = names[i * 4];
                        SettingsPropertyValue value2 = properties[str];
                        if (value2 != null)
                        {
                            int startIndex = int.Parse(names[(i * 4) + 2], CultureInfo.InvariantCulture);
                            int length = int.Parse(names[(i * 4) + 3], CultureInfo.InvariantCulture);
                            if ((length == -1) && !value2.Property.PropertyType.IsValueType)
                            {
                                value2.PropertyValue = null;
                                value2.IsDirty = false;
                                value2.Deserialized = true;
                            }
                            if (((names[(i * 4) + 1] == "S") && (startIndex >= 0)) && ((length > 0) && (values.Length >= (startIndex + length))))
                            {
                                value2.SerializedValue = values.Substring(startIndex, length);
                            }
                            if (((names[(i * 4) + 1] == "B") && (startIndex >= 0)) && ((length > 0) && (buf.Length >= (startIndex + length))))
                            {
                                byte[] dst = new byte[length];
                                Buffer.BlockCopy(buf, startIndex, dst, 0, length);
                                value2.SerializedValue = dst;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            if (properties.Count == 0)
                return svc;

            string[] names = null;
            string values = null;

            //Create the default structure of the properties
            foreach (SettingsProperty prop in properties)
            {
                if (prop.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                        prop.SerializeAs = SettingsSerializeAs.String;
                    else
                        prop.SerializeAs = SettingsSerializeAs.Xml;

                svc.Add(new SettingsPropertyValue(prop));
            }

            Profile dbProperties = NHibernateProviderEntityHelper.GetProfile((string)context["UserName"]);
            if (dbProperties != null)
            {
                names = dbProperties.PropertyNames.Split(':');
                values = dbProperties.PropertyValuesString;

                if (names != null && names.Length > 0)
                {
                    ParseDataFromDB(names, values, dbProperties.PropertyValuesBinary, svc);
                }
            }

            return svc;
        }

        public override void SetPropertyValues(System.Configuration.SettingsContext sc, SettingsPropertyValueCollection properties)
        {
            string objValue = (string)sc["UserName"];
            bool userIsAuthenticated = (bool)sc["IsAuthenticated"];
            if (((objValue != null) && (objValue.Length >= 1)) && (properties.Count >= 1))
            {
                string allNames = string.Empty;
                string allValues = string.Empty;
                byte[] buf = null;
                PrepareDataForSaving(ref allNames, ref allValues, ref buf, true, properties, userIsAuthenticated);
                if (allNames.Length != 0)
                {
                    Profile p = new Profile();
                    p.UserId = NHibernateProviderEntityHelper.GetUser(objValue).Id;
                    p.PropertyNames = allNames;
                    p.PropertyValuesBinary = buf;
                    p.PropertyValuesString = allValues;
                    p.LastActivityDate = DateTime.Now;

                    if (NHibernateProviderEntityHelper.GetProfile(objValue) == null)
                        NHibernateHelper.Save(p);
                    else
                        NHibernateHelper.Update(p);
                }
            }
        }

        void PrepareDataForSaving(ref string allNames, ref string allValues, ref byte[] buf, bool binarySupported, SettingsPropertyValueCollection properties, bool userIsAuthenticated)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            MemoryStream stream = binarySupported ? new MemoryStream() : null;
            try
            {
                try
                {
                    bool flag = false;
                    foreach (SettingsPropertyValue value2 in properties)
                    {
                        if (value2.IsDirty && (userIsAuthenticated || ((bool)value2.Property.Attributes["AllowAnonymous"])))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        return;
                    }
                    foreach (SettingsPropertyValue value3 in properties)
                    {
                        if ((!userIsAuthenticated && !((bool)value3.Property.Attributes["AllowAnonymous"])) || (!value3.IsDirty && value3.UsingDefaultValue))
                        {
                            continue;
                        }
                        int length = 0;
                        int position = 0;
                        string str = null;
                        if (value3.Deserialized && (value3.PropertyValue == null))
                        {
                            length = -1;
                        }
                        else
                        {
                            object serializedValue = value3.SerializedValue;
                            if (serializedValue == null)
                            {
                                length = -1;
                            }
                            else
                            {
                                if (!(serializedValue is string) && !binarySupported)
                                {
                                    serializedValue = Convert.ToBase64String((byte[])serializedValue);
                                }
                                if (serializedValue is string)
                                {
                                    str = (string)serializedValue;
                                    length = str.Length;
                                    position = builder2.Length;
                                }
                                else
                                {
                                    byte[] buffer = (byte[])serializedValue;
                                    position = (int)stream.Position;
                                    stream.Write(buffer, 0, buffer.Length);
                                    stream.Position = position + buffer.Length;
                                    length = buffer.Length;
                                }
                            }
                        }
                        builder.Append(value3.Name + ":" + ((str != null) ? "S" : "B") + ":" + position.ToString(CultureInfo.InvariantCulture) + ":" + length.ToString(CultureInfo.InvariantCulture) + ":");
                        if (str != null)
                        {
                            builder2.Append(str);
                        }
                    }
                    if (binarySupported)
                    {
                        buf = stream.ToArray();
                    }
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
            allNames = builder.ToString();
            allValues = builder2.ToString();
        }





    }
}
