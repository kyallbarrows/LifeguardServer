using System;
using System.Collections.Generic;

using System.Text;
using System.Web;

public static class NavHelper
{
    #region Navigation Helpers

    public static string BaseURL
    {
        get { return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + '/'; }
    }

    /// <summary>
    /// Retrieves Physical Application Path for a location
    /// For example "C:\Users\Sam\Documents\Visual Studio 2008\WebSites\Parichay.Web\Default.Aspx" for the path "Default.aspx"
    /// </summary>
    /// <param name="path">Physical Path to be appended</param>
    /// <returns></returns>
    public static string GetPhysicalLocation(string path)
    {
        return (System.Web.HttpContext.Current.Request.PhysicalApplicationPath + path);
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// Build a url based on the page and the parameters specified
    /// </summary>
    /// <param name="page">The page, like page.aspx</param>
    /// <param name="paramTemplate">Must be a format string like forum={0}&id={1}</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string BuildUrl(string page, string paramTemplate, params string[] parameters)
    {
        string[] encodedParams;
        if (parameters != null && parameters.Length > 0)
        {
            encodedParams = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                encodedParams[i] = HttpUtility.UrlEncode(parameters[i]);
        }
        else
        {
            encodedParams = new string[0];
        }

        return page + "?" + string.Format(paramTemplate, encodedParams);
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// If the path is absolute is return as is, otherwise is combined with AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    /// The path are always server relative path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LocateServerPath(string path)
    {
        if (System.IO.Path.IsPathRooted(path) == false)
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);

        return path;
    }


    private static string CombineUrl(string baseUrl, string relativeUrl)
    {
        if (relativeUrl.Length == 0 || relativeUrl[0] != '/')
            relativeUrl = '/' + relativeUrl;

        if (baseUrl.Length > 0 && baseUrl[baseUrl.Length - 1] == '/')
            baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);

        return baseUrl + relativeUrl;

        //Uri uriBase = new Uri(baseUrl);
        //Uri uriRelative = new Uri(relativeUrl, UriKind.Relative);
        //Uri uri = new Uri(uriBase, uriRelative);

        //return uri.AbsoluteUri;
    }


    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// Returns an Url to this page that can be used on server side (using ~ notation)
    /// </summary>
    /// <param name="htmlEncode">True to encode the url so can be used without problems inside an xhtml article</param>
    /// <returns></returns>
    public static string GetServerUrl(string Location, bool htmlEncode)
    {
        string url = CombineUrl("~", Location);
        if (htmlEncode)
            url = HttpUtility.HtmlEncode(url);
        return url;
    }

    public static string GetServerUrl(string Location, string Id, bool htmlEncode)
    {
        return BuildUrl(Location, "id={0}", Id);
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// Returns an absolute client url
    /// </summary>
    /// <param name="htmlEncode">True to encode the url so can be used without problems inside an xhtml article</param>
    /// <returns></returns>
    public static string GetAbsoluteClientUrl(string Location, bool htmlEncode)
    {
        string url = CombineUrl(BaseURL, Location);
        if (htmlEncode)
            url = HttpUtility.HtmlEncode(url);
        return url;
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// Returns an url to this page that can be used on client side
    /// </summary>
    /// <param name="source"></param>
    /// <param name="htmlEncode">True to encode the url so can be used without problems inside an xhtml article</param>
    /// <returns></returns>
    public static string GetClientUrl(System.Web.UI.Control source, string targetLocation, bool htmlEncode)
    {
        string url = source.ResolveClientUrl(GetServerUrl(targetLocation, false));
        if (htmlEncode)
            url = HttpUtility.HtmlEncode(url);
        return url;
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// Redirect the response to this page.
    /// </summary>
    /// <param name="source"></param>
    public static void Redirect(System.Web.UI.Control source, string targetLocation)
    {
        source.Page.Response.Redirect(GetServerUrl(targetLocation, false));
    }

    /// <summary>
    /// Utility method taken from Eucalypto http: // www.codeproject.com/KB/aspnet/eucalypto.aspx
    /// User Server.Transfer.
    /// </summary>
    public static void Transfer(string targetLocation)
    {
        System.Web.HttpContext.Current.Server.Transfer(GetServerUrl(targetLocation, false));
    }
    #endregion Navigation Helpers
}
