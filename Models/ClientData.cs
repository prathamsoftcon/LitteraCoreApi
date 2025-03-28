using Newtonsoft.Json;
using System.Xml;

namespace LitteraCore.Models
{
    public class ClientData
    {
        public string name { get; set; }
        public string logo { get; set; }
        public string title { get; set; }
        public string icon { get; set; }

        public string LOGIN_LOGO { get; set; }

        public string CERTIFICATE_TXT { get; set; }

        public string ACHIEVEMENT_FIRST_BOX_HEADER { get; set; }

        public string ACHIEVEMENT_FIRST_BOX_VALUE { get; set; }

        public string ACHIEVEMENT_SECOND_BOX_HEADER { get; set; }

        public string ACHIEVEMENT_SECOND_BOX_VALUE { get; set; }

        public string ACHIEVEMENT_THIRD_BOX_HEADER { get; set; }

        public string ACHIEVEMENT_THIRD_BOX_VALUE { get; set; }

        public string ACHIEVEMENT_FOURTH_BOX_HEADER { get; set; }

        public string ACHIEVEMENT_FOURTH_BOX_VALUE { get; set; }

        public string QR_IMG { get; set; }

        public string FB_LINK { get; set; }

        public string TWT_LINK { get; set; }

        public string LINKEDIN_LINK { get; set; }

        public string SLIDER_1_IMG { get; set; }

        public string SLIDER_2_IMG { get; set; }

        public string SLIDER_3_IMG { get; set; }

        public string WELCOME_TXT_HEADER { get; set; }

        public string WELCOME_TXT_DETAILS { get; set; }

        public string CERTIFIED_FACULTY_TEXT { get; set; }

        public string CERTIFICATION_TEXT { get; set; }

        public string ABT_US_VIDEO { get; set; }

        public string ABT_US_VIDEO_IMG { get; set; }

        public string ABT_US_MAIN_TXT { get; set; }

        public string ABT_US_WELCOME_HEADER { get; set; }

        public string ABT_US_WELCOME_DETAIL { get; set; }

        public string ABT_US_CONCEPT_HEADER { get; set; }

        public string ABT_US_CONCEPT_DETAIL { get; set; }

        public string ABT_US_MISSION_HEADER { get; set; }

        public string ABT_US_MISSION_DETAILS { get; set; }

        public string ABT_US_VISION_HEADER { get; set; }

        public string ABT_US_VISION_DETAIL { get; set; }

        public string ACHEIVEMENT_TEXT { get; set; }

        public string CONTACTUS_ADDRESS { get; set; }

        public string CONTACTUS_PHONENO { get; set; }

        public string ACHEIVEMENT_IMG { get; set; }

        public string CONTACTUS_EMAIL { get; set; }

        public string READ_MORE_PAGE_HEADER { get; set; }

        public string READ_MORE_PAGE_DETAILS { get; set; }

        public string READ_MORE_PAGE_VIDEO { get; set; }

        public string READ_MORE_PAGE_GOALS { get; set; }

        public string PRIVACY_PAGE_HEADER { get; set; }

        public string PRIVACY_PAGE_CONTENT { get; set; }
        public string SLIDER_READ_MORE_DISPLAY { get; set; }

        public string LITTERA_SITE_TAGLINE { get; set; }

        public string LITTERA_LOGO { get; set; }

        public string DASHBOARD_LOGO { get; set; }

        public string APP_TITLE { get; set; }

        public string FAV_ICON { get; set; }


        public string TRENDING_TRG_Header { get; set; }

        public string TRG_CALENDAR_TEXT { get; set; }

        public string TRENDING_TRG_TEXT { get; set; }

        public string APP_LOGIN_LOGO { get; set; }
        public string APP_OTP_LOGO { get; set; }
        public string APP_LOGIN_TITLE { get; set; }
        public string APP_OTP_TITLE { get; set; }
        public string APP_DASHBOARD_TITLE { get; set; }

        public string CERTIFICATE_LOGO { get; set; }

        public string INSTRUCTION_URL { get; set; }

        public Menus[] MENULIST { get; set; }

        

        public static  ClientData Get_Client_Data()
        {
            XmlDocument xmldoc = new XmlDocument();
            string Foldername = "Content/GlobalSetting";
            //xmldoc.Load(System.Web.HttpContext.Current.Server.MapPath("~/" + Foldername + "/ClientData.xml"));
            xmldoc.Load(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Content/GlobalSetting", "ClientData.xml"));
            XmlNodeList Nodes = xmldoc.DocumentElement.SelectNodes("/ClientData/setting");
            String name = "", logo = "", icon = "", title = "";
            var json = JsonConvert.SerializeXmlNode(xmldoc.ChildNodes[1], Newtonsoft.Json.Formatting.Indented, true);
            //JObject JS= JObject.Parse(XmlNodeList);


            ClientData cinfo = new ClientData();
            foreach (XmlNode node in Nodes)
            {
                if (node.SelectSingleNode("key").InnerText == "LOGIN_LOGO")
                {
                    cinfo.logo = node.SelectSingleNode("value").InnerText;
                    cinfo.LOGIN_LOGO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CERTIFICATE_TXT")
                {
                    cinfo.CERTIFICATE_TXT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_FIRST_BOX_HEADER")
                {
                    cinfo.ACHIEVEMENT_FIRST_BOX_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_FIRST_BOX_VALUE")
                {
                    cinfo.ACHIEVEMENT_FIRST_BOX_VALUE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_SECOND_BOX_HEADER")
                {
                    cinfo.ACHIEVEMENT_SECOND_BOX_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_SECOND_BOX_VALUE")
                {
                    cinfo.ACHIEVEMENT_SECOND_BOX_VALUE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_THIRD_BOX_HEADER")
                {
                    cinfo.ACHIEVEMENT_THIRD_BOX_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_THIRD_BOX_VALUE")
                {
                    cinfo.ACHIEVEMENT_THIRD_BOX_VALUE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_FOURTH_BOX_HEADER")
                {
                    cinfo.ACHIEVEMENT_FOURTH_BOX_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHIEVEMENT_FOURTH_BOX_VALUE")
                {
                    cinfo.ACHIEVEMENT_FOURTH_BOX_VALUE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "QR_IMG")
                {
                    cinfo.QR_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "FB_LINK")
                {
                    cinfo.FB_LINK = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "TWT_LINK")
                {
                    cinfo.TWT_LINK = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "LINKEDIN_LINK")
                {
                    cinfo.LINKEDIN_LINK = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "SLIDER_1_IMG")
                {
                    cinfo.SLIDER_1_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "SLIDER_2_IMG")
                {
                    cinfo.SLIDER_2_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "SLIDER_3_IMG")
                {
                    cinfo.SLIDER_3_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "WELCOME_TXT_HEADER")
                {
                    cinfo.WELCOME_TXT_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "WELCOME_TXT_DETAILS")
                {
                    cinfo.WELCOME_TXT_DETAILS = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CERTIFIED_FACULTY_TEXT")
                {
                    cinfo.CERTIFIED_FACULTY_TEXT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CERTIFICATION_TEXT")
                {
                    cinfo.CERTIFICATION_TEXT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_VIDEO")
                {
                    cinfo.ABT_US_VIDEO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_VIDEO_IMG")
                {
                    cinfo.ABT_US_VIDEO_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_MAIN_TXT")
                {
                    cinfo.ABT_US_MAIN_TXT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_WELCOME_HEADER")
                {
                    cinfo.ABT_US_WELCOME_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_WELCOME_DETAIL")
                {
                    cinfo.ABT_US_WELCOME_DETAIL = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_CONCEPT_HEADER")
                {
                    cinfo.ABT_US_CONCEPT_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_CONCEPT_DETAIL")
                {
                    cinfo.ABT_US_CONCEPT_DETAIL = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_MISSION_HEADER")
                {
                    cinfo.ABT_US_MISSION_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_MISSION_DETAILS")
                {
                    cinfo.ABT_US_MISSION_DETAILS = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_VISION_HEADER")
                {
                    cinfo.ABT_US_VISION_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ABT_US_VISION_DETAIL")
                {
                    cinfo.ABT_US_VISION_DETAIL = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHEIVEMENT_TEXT")
                {
                    cinfo.ACHEIVEMENT_TEXT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CONTACTUS_ADDRESS")
                {
                    cinfo.CONTACTUS_ADDRESS = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CONTACTUS_PHONENO")
                {
                    cinfo.CONTACTUS_PHONENO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "ACHEIVEMENT_IMG")
                {
                    cinfo.ACHEIVEMENT_IMG = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "CONTACTUS_EMAIL")
                {
                    cinfo.CONTACTUS_EMAIL = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "READ_MORE_PAGE_HEADER")
                {
                    cinfo.READ_MORE_PAGE_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "READ_MORE_PAGE_DETAILS")
                {
                    cinfo.READ_MORE_PAGE_DETAILS = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "READ_MORE_PAGE_VIDEO")
                {
                    cinfo.READ_MORE_PAGE_VIDEO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "READ_MORE_PAGE_GOALS")
                {
                    cinfo.READ_MORE_PAGE_GOALS = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "PRIVACY_PAGE_HEADER")
                {
                    cinfo.PRIVACY_PAGE_HEADER = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "PRIVACY_PAGE_CONTENT")
                {
                    cinfo.PRIVACY_PAGE_CONTENT = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "SLIDER_READ_MORE_DISPLAY")
                {
                    cinfo.SLIDER_READ_MORE_DISPLAY = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "LITTERA_SITE_TAGLINE")
                {
                    cinfo.LITTERA_SITE_TAGLINE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "LITTERA_LOGO")
                {
                    cinfo.LITTERA_LOGO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "DASHBOARD_LOGO")
                {
                    cinfo.DASHBOARD_LOGO = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "APP_TITLE")
                {
                    cinfo.title = node.SelectSingleNode("value").InnerText;
                    cinfo.APP_TITLE = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "FAV_ICON")
                {
                    cinfo.FAV_ICON = node.SelectSingleNode("value").InnerText;
                    cinfo.icon = node.SelectSingleNode("value").InnerText;
                }
                else if (node.SelectSingleNode("key").InnerText == "TRENDING_TRG_Header")
                {
                    cinfo.TRENDING_TRG_Header = node.SelectSingleNode("value").InnerText;

                }
                else if (node.SelectSingleNode("key").InnerText == "TRG_CALENDAR_TEXT")
                {
                    cinfo.TRG_CALENDAR_TEXT = node.SelectSingleNode("value").InnerText;

                }
                else if (node.SelectSingleNode("key").InnerText == "TRENDING_TRG_TEXT")
                {
                    cinfo.TRENDING_TRG_TEXT = node.SelectSingleNode("value").InnerText;

                }
                else if (node.SelectSingleNode("key").InnerText == "CERTIFICATE_LOGO")
                {
                    cinfo.CERTIFICATE_LOGO = node.SelectSingleNode("value").InnerText;

                }
                else if (node.SelectSingleNode("key").InnerText == "INSTRUCTION_URL")
                {
                    cinfo.INSTRUCTION_URL = node.SelectSingleNode("value").InnerText;

                }
                else if (node.SelectSingleNode("key").InnerText == "MENULIST")
                {
                    if(node.SelectSingleNode("value").InnerText != "")
                    {
                        cinfo.MENULIST = JsonConvert.DeserializeObject<Menus[]>(node.SelectSingleNode("value").InnerText);
                    }
                   

                }




            }
            ClientData c = new ClientData
            {
                name = name,
                logo = logo,
                icon = title,
                title = icon
            };



            return cinfo;
        }
    }
    
    public class Menus
    {
        public string menu_name { get; set;}
        public string redirect_url { get; set; }
        public bool redirect_self_window { get; set; }
    }
 


}
