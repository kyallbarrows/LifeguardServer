using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace Parichay
{
    public static class AppConstants
    {
        public const string adminEmail = "admin@parichay.com";

        public static readonly Dictionary<string, string> lstGender =  new Dictionary<string, string>() { { "M", "Male" }, { "F", "Female" }}; 
        public static readonly Dictionary<string, string> lstTitles =  new Dictionary<string, string>() { { "Mr.", "Mr" }, { "Mrs.", "Mrs." }, { "Prof.", "Prof." }, { "Dr.", "Dr." }, { "Ms.", "Ms." }};
        public static readonly Dictionary<int, string> lstGroupRoles = new Dictionary<int, string>() { { 0, "Member" }, { 1, "Admin" }, { 2, "Owner" } };
        public static readonly Dictionary<int, string> lstAccessTypes = new Dictionary<int, string>() { { 0, "Public" }, { 1, "Friends" }, { 2, "Self" } };
        public enum InviteTypes { Join = 0, Friend = 1, Group = 2 }
        public  enum RequestTypes  { Join=0, Friend=1 , Group=2 }
        public enum ReturnContollerHomes { Account = 0, Message = 1, Friend = 3, Group = 4, Media = 5, Alert = 6, Connect = 7, Admin = 8 } 
        public static string BaseSiteUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
                return baseUrl;
            }
        }
        public static readonly Dictionary<string,string> lstCountries = new Dictionary<string,string>(){
                        {"00","NA" },
                        {"99","ALL" },
                        {"AD","Andorra" },
                        {"AE","United Arab Emirates" },
                        {"AF","Afghanistan" },
                        {"AG","Antigua & Barbuda" },
                        {"AI","Anguilla" },
                        {"AL","Albania" },
                        {"AM","Armenia" },
                        {"AN","Netherlands Antilles" },
                        {"AO","Angola" },
                        {"AQ","Antarctica" },
                        {"AR","Argentina" },
                        {"AT","Austria" },
                        {"AU","Australia" },
                        {"AW","Aruba" },
                        {"AZ","Azerbaijan" },
                        {"BA","Bosnia and Herzegovina" },
                        {"BB","Barbados" },
                        {"BD","Bangladesh" },
                        {"BE","Belgium" },
                        {"BF","Burkina Faso" },
                        {"BG","Bulgaria" },
                        {"BH","Bahrain" },
                        {"BI","Burundi" },
                        {"BJ","Benin" },
                        {"BM","Bermuda" },
                        {"BN","Brunei" },
                        {"BO","Bolivia" },
                        {"BR","Brazil" },
                        {"BS","Bahamas" },
                        {"BT","Bhutan" },
                        {"BV","Bouvet Island" },
                        {"BW","Botswana" },
                        {"BY","Belarus" },
                        {"BZ","Belize" },
                        {"CA","Canada" },
                        {"CC","Cocos (Keeling) Islands" },
                        {"CF","Central African Republic" },
                        {"CG","Congo" },
                        {"CH","Switzerland" },
                        {"CI","Ivory Coast" },
                        {"CK","Cook Islands" },
                        {"CL","Chile" },
                        {"CM","Cameroon" },
                        {"CN","China" },
                        {"CO","Colombia" },
                        {"CR","Costa Rica" },
                        {"CU","Cuba" },
                        {"CV","Cape Verde" },
                        {"CX","Christmas Island" },
                        {"CY","Cyprus" },
                        {"CZ","Czech Republic" },
                        {"DE","Germany" },
                        {"DJ","Djibouti" },
                        {"DK","Denmark" },
                        {"DM","Dominica" },
                        {"DO","Dominican Republic" },
                        {"DZ","Algeria" },
                        {"EC","Ecuador" },
                        {"EE","Estonia" },
                        {"EG","Egypt" },
                        {"EH","Western Sahara" },
                        {"ER","Eritrea" },
                        {"ES","Spain" },
                        {"ET","Ethiopia" },
                        {"FI","Finland" },
                        {"FJ","Fiji" },
                        {"FK","Falkland Islands" },
                        {"FO","Faroe Islands" },
                        {"FR","France" },
                        {"GA","Gabon" },
                        {"GB","United Kingdom" },
                        {"GD","Grenada" },
                        {"GE","Georgia" },
                        {"GF","French Guiana" },
                        {"GH","Ghana" },
                        {"GI","Gibraltar" },
                        {"GL","Greenland" },
                        {"GM","Gambia" },
                        {"GN","Guinea" },
                        {"GP","Guadeloupe" },
                        {"GQ","Equatorial Guinea" },
                        {"GR","Greece" },
                        {"GS","Sth Georgia & Sth Sandwich Isl" },
                        {"GT","Guatemala" },
                        {"GW","Guinea-Bissau" },
                        {"GY","Guyana" },
                        {"HK","Hong Kong" },
                        {"HM","Heard and Mcdonald Islands" },
                        {"HN","Honduras" },
                        {"HR","Croatia" },
                        {"HT","Haiti" },
                        {"HU","Hungary" },
                        {"ID","Indonesia" },
                        {"IE","Ireland" },
                        {"IL","Israel" },
                        {"IN","India" },
                        {"IO","British Indian Ocean Territory" },
                        {"IQ","Iraq" },
                        {"IR","Iran" },
                        {"IS","Iceland" },
                        {"IT","Italy" },
                        {"JM","Jamaica" },
                        {"JO","Jordan" },
                        {"JP","Japan" },
                        {"KE","Kenya" },
                        {"KG","Kyrgyzstan" },
                        {"KH","Cambodia" },
                        {"KI","Kiribati" },
                        {"KM","Comoros" },
                        {"KN","St. Kitts and Nevis" },
                        {"KP","North Korea" },
                        {"KR","Korea" },
                        {"KW","Kuwait" },
                        {"KY","Cayman Islands" },
                        {"KZ","Kazakhstan" },
                        {"LA","Laos" },
                        {"LB","Lebanon" },
                        {"LC","St. Lucia" },
                        {"LI","Liechtenstein" },
                        {"LK","Sri Lanka" },
                        {"LR","Liberia" },
                        {"LS","Lesotho" },
                        {"LT","Lithuania" },
                        {"LU","Luxembourg" },
                        {"LV","Latvia" },
                        {"LY","Libya" },
                        {"MA","Morocco" },
                        {"MC","Monaco" },
                        {"MD","Moldova" },
                        {"MG","Madagascar" },
                        {"MK","FYROM" },
                        {"ML","Mali" },
                        {"MM","Myanmar (Burma)" },
                        {"MN","Mongolia" },
                        {"MO","Macau" },
                        {"MQ","Martinique" },
                        {"MR","Mauritania" },
                        {"MS","Montserrat" },
                        {"MT","Malta" },
                        {"MU","Mauritius" },
                        {"MV","Maldives" },
                        {"MW","Malawi" },
                        {"MX","Mexico" },
                        {"MY","Malaysia" },
                        {"MZ","Mozambique" },
                        {"NA","Namibia" },
                        {"NC","New Caledonia" },
                        {"NE","Niger" },
                        {"NF","Norfolk Island" },
                        {"NG","Nigeria" },
                        {"NI","Nicaragua" },
                        {"NL","Netherlands" },
                        {"NO","Norway" },
                        {"NP","Nepal" },
                        {"NR","Nauru" },
                        {"NU","Niue" },
                        {"NZ","New Zealand" },
                        {"OM","Oman" },
                        {"PA","Panama" },
                        {"PE","Peru" },
                        {"PF","French Polynesia" },
                        {"PG","Papua New Guinea" },
                        {"PH","Philippines" },
                        {"PK","Pakistan" },
                        {"PL","Poland" },
                        {"PM","St. Pierre and Miquelon" },
                        {"PN","Pitcairn" },
                        {"PT","Portugal" },
                        {"PY","Paraguay" },
                        {"QA","Qatar" },
                        {"RE","Reunion" },
                        {"RO","Romania" },
                        {"RU","Russia" },
                        {"RW","Rwanda" },
                        {"SA","Saudi Arabia" },
                        {"SB","Solomon Islands" },
                        {"SC","Seychelles" },
                        {"SD","Sudan" },
                        {"SE","Sweden" },
                        {"SG","Singapore" },
                        {"SH","Saint Helena" },
                        {"SI","Slovenia" },
                        {"SJ","Svalbard and Jan Mayen" },
                        {"SK","Slovakia" },
                        {"SL","Sierra Leone" },
                        {"SM","San Marino" },
                        {"SN","Senegal" },
                        {"SO","Somalia" },
                        {"SR","Suriname" },
                        {"ST","St. Tome and Principe" },
                        {"SV","El Salvador" },
                        {"SY","Syria" },
                        {"SZ","Swaziland" },
                        {"TC","Turks & Caicos Isl (BWI)" },
                        {"TD","Chad" },
                        {"TF","French Southern Territories" },
                        {"TG","Togo" },
                        {"TH","Thailand" },
                        {"TJ","Tajikistan" },
                        {"TK","Tokelau" },
                        {"TM","Turkmenistan" },
                        {"TN","Tunisia" },
                        {"TO","Tonga" },
                        {"TP","East Timor" },
                        {"TR","Turkey" },
                        {"TT","Trinidad and Tobago" },
                        {"TV","Tuvalu" },
                        {"TW","Taiwan" },
                        {"TZ","Tanzania" },
                        {"UA","Ukraine" },
                        {"UG","Uganda" },
                        {"UM","US Minor Outlying Islands" },
                        {"USA","USA" },
                        {"UY","Uruguay" },
                        {"UZ","Uzbekistan" },
                        {"VA","Vatican City" },
                        {"VC","St. Vincent and the Grenadines" },
                        {"VE","Venezuela" },
                        {"VG","Virgin Islands (British)" },
                        {"VN","Vietnam" },
                        {"VU","Vanuatu" },
                        {"WF","Wallis & Futuna Islands" },
                        {"WS","Samoa" },
                        {"YE","Yemen Republic" },
                        {"YT","Mayotte" },
                        {"YU","Yugoslavia" },
                        {"ZA","South Africa" },
                        {"ZM","Zambia" },
                        {"ZW","Zimbabwe" },
                        {"ZZ","Unknown" }
                            };
    }
    public static class EnumUtils
    {
        public static string stringValueOf(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static object enumValueOf(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (stringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }
    }
}