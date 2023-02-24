<%@ Application Language="C#" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="log4net.Config" %>
<%@ Import Namespace="log4net.Appender" %>
<%@ Import Namespace="log4net.Layout" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RollingFileAppender rfa = new RollingFileAppender();
        //rfa.File = "c:\\src\\draft\\modelcrm\\logs\\modelcrm.log";
        rfa.File = "logs\\modelcrm.log";
        rfa.StaticLogFileName = true;
        rfa.RollingStyle = RollingFileAppender.RollingMode.Size;
        rfa.MaxSizeRollBackups = 10;
        rfa.MaximumFileSize = "1MB";

        PatternLayout pl = new PatternLayout();
        pl.ConversionPattern = "%date [%thread] %-5level %logger [%property{NDC}] - %message%newline";
        rfa.Layout = pl;

        rfa.ActivateOptions();

        BasicConfigurator.Configure(rfa);

        ILog log = LogManager.GetLogger("Globals.asax");

        log.Info("modelcrm started");
        #region Admin Rolü yoksa...

        if (Roles.Provider.ApplicationName != Membership.ApplicationName)
            Roles.Provider.ApplicationName = Membership.ApplicationName;
        MembershipUserCollection muc = Membership.FindUsersByName("Admin");
        if (muc.Count == 0)
        {
            MembershipCreateStatus status;
            Membership.CreateUser("Admin", "1Admin9", "admin@mbi.com.tr", null, null, true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                System.Data.SqlClient.SqlConnection conn = Model.Crm.DB.Connect();

                System.Data.SqlClient.SqlCommand cmd = Model.Crm.DB.SQL(conn, "SELECT UserId FROM aspnet_Users WHERE UserName=@UserName");
                Model.Crm.DB.AddParam(cmd, "@UserName", 255, "Admin");
                cmd.Prepare();
                Guid gUserID = (Guid)cmd.ExecuteScalar();

                System.Text.StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO SecurityUsers(UserID,UserName,Password,FirstName,LastName,Email,Title,Department,CreatedBy,CreationDate)");
                sb.Append("VALUES(@UserID,@UserName,@Password,@FirstName,@LastName,@Email,@Title,@Department,@CreatedBy,@CreationDate)");
                cmd = Model.Crm.DB.SQL(this.Context, sb.ToString());
                Model.Crm.DB.AddParam(cmd, "@UserID", gUserID);
                Model.Crm.DB.AddParam(cmd, "@UserName", 100, "Admin");
                Model.Crm.DB.AddParam(cmd, "@Password", 100, "1Admin9");
                Model.Crm.DB.AddParam(cmd, "@FirstName", 60, "Admin");
                Model.Crm.DB.AddParam(cmd, "@LastName", 60, "Admin");
                Model.Crm.DB.AddParam(cmd, "@Email", 255, "admin@mbi.com.tr");
                Model.Crm.DB.AddParam(cmd, "@Title", 255, "Admin");
                Model.Crm.DB.AddParam(cmd, "@Department", 255, "Admin");
                Model.Crm.DB.AddParam(cmd, "@CreatedBy", 60, "Admin");
                Model.Crm.DB.AddParam(cmd, "@CreationDate", DateTime.Now);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        Model.Crm.ApplicationUtils service = new Model.Crm.ApplicationUtils();
        if (service != null)
        {
            service.GetAppSettings();
            service.RemoveCaches();
            service.RegisterServiceCache();
        }
        #endregion
    }

    void Application_BeginRequest(Object sender, EventArgs e)
    {
        // If the dummy page is hit, then it means we want to add another item in cache
        if ((HttpContext.Current.Request.Url != null) && (ConfigurationManager.AppSettings["HitPageUrl"] != null))
        {
            if (HttpContext.Current.Request.Url.ToString() == ConfigurationManager.AppSettings["HitPageUrl"].ToString())
            {
                Model.Crm.ApplicationUtils service = new Model.Crm.ApplicationUtils();
                if (service != null) service.RegisterServiceCache();
            }
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        try
        {
            Server.ScriptTimeout = 300;
            if (Roles.Provider.ApplicationName != Membership.ApplicationName)
                Roles.Provider.ApplicationName = Membership.ApplicationName;
            // Code that runs when a new session is started
            //Session.Clear();
            DataSetViewState ds = new DataSetViewState();
            Session["dsViewState"] = ds;
            Session["Permissions"] = null;
            ILog log = LogManager.GetLogger("Globals.asax");

            //log.Info("ÖRole:");
            //log.Info(Roles.Provider.ApplicationName);
            //log.Info("ÖMemberShip:");
            //log.Info(Membership.ApplicationName);

            //log.Info("SRole:");
            //log.Info(Roles.Provider.ApplicationName);
            //log.Info("SMemberShip:");
            //log.Info(Membership.ApplicationName);

        }
        catch (Exception ex)
        {
            ConfigurationManager.AppSettings.Set("Problem", ex.Message);
        }
        finally
        {
            //Model.Crm.ApplicationUtils service = new Model.Crm.ApplicationUtils();
            //if (service != null)
            //{
            //    service.GetAppSettings();
            //    service.RemoveCaches();
            //    service.RegisterServiceCache();
            //}
        }

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>

