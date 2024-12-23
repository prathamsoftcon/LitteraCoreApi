using MimeKit;
using MailKit.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Xml;
using Microsoft.AspNetCore.Components.Routing;
using System.Text;
using LitteraCore.Models;

namespace LitteraCore.Common.EmailService
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _clientname;
        private readonly string _clienturl;
        private readonly string _header;
        private readonly string _footer;
        public SmtpEmailService(IConfiguration config)
        {
            _smtpServer = Get_Email_Conf("host");   //config["SmtpSettings:Server"];
            _smtpPort = Get_Email_Conf("portno");  //int.Parse(config["SmtpSettings:Port"]);
            _smtpUsername = Get_Email_Conf("login");//config["SmtpSettings:smtpUsername"];
            _smtpPassword = Get_Email_Conf("password"); //config["SmtpSettings:smtpPassword"];
            _clientname = Get_Email_Conf("clientname");
            _clienturl = Get_Email_Conf("clienturl");
            _header = Get_Email_Conf("header");
            _footer = Get_Email_Conf("footer");
        }

        //public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        //{
        //    try
        //    {
        //        // Append header and footer to the message
        //        //message = _header + message + _footer;

        //        //MailMessage msg = new MailMessage();
        //        //SmtpClient smtpServer = new SmtpClient();





        //        //string mailid = _smtpUsername;
        //        //string pwd = _smtpPassword;



        //        //smtpServer.Credentials = new System.Net.NetworkCredential(mailid, pwd);
        //        //smtpServer.Port = Convert.ToInt32(_smtpPort);
        //        //smtpServer.Host = _smtpServer;
        //        //smtpServer.EnableSsl = true;

        //        //msg.To.Add(recipientEmail);
        //        //msg.From = new MailAddress(mailid, _clientname, Encoding.UTF8);
        //        //msg.Subject = subject;
        //        //msg.Body = message;
        //        //msg.IsBodyHtml = true;
        //        //msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        //        //msg.ReplyToList.Add(new MailAddress(mailid));

        //        //smtpServer.Send(msg);

        //        using (var client = new SmtpClient(_smtpServer, Convert.ToInt32(_smtpPort)))
        //        {
        //            // Do not use default credentials

        //            client.EnableSsl = true;
        //            client.UseDefaultCredentials = false;

        //            // Set credentials
        //            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

        //            // Enable SSL


        //            using (var email = new MailMessage())
        //            {
        //                // Set email properties
        //                email.From = new MailAddress(_smtpUsername);
        //                email.To.Add(recipientEmail);
        //                email.Subject = subject;
        //                email.Body = message;
        //                email.IsBodyHtml = true;
        //                client.SendCompleted += (sender, e) =>
        //                {
        //                    Console.WriteLine($"Message sent: {e.UserState}");
        //                    if (e.Error != null)
        //                    {
        //                        Console.WriteLine($"Error: {e.Error}");
        //                    }
        //                };
        //                // Send email
        //                await client.SendMailAsync(email);
        //            }
        //        }
        //    }
        //    catch (SmtpException ex)
        //    {
        //        // Log or handle the SMTP exception
        //        Console.WriteLine($"SMTP Error: {ex.Message}");
        //        throw; // Optionally rethrow the exception to be handled by calling code
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the general exception
        //        Console.WriteLine($"General Error: {ex.Message}");
        //        throw; // Optionally rethrow the exception to be handled by calling code
        //    }
        //}

        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_smtpUsername);
            email.To.Add(MailboxAddress.Parse(recipientEmail));
            email.Subject = subject;
            var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = message;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpServer,Convert.ToInt32(_smtpPort), SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpUsername, _smtpPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public string Get_Email_Conf(string key)
        {
            string clientname = "";
            string clienturl = "";
            string host = "";
            string Port = "";
            string username = "";
            string password = "";
            string header = "";
            string footer = "";
            string returnval = "";
            // Code to Get SMS Settings
            XmlDocument xmldoc = new XmlDocument();
            // xmldoc.Load(@"StaticFiles/emailSetting.xml");
            xmldoc.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/GlobalSetting", "emailSetting.xml"));
            XmlNodeList Nodes = xmldoc.DocumentElement.SelectNodes("/EmailConfiguration");
            foreach (XmlNode Node in Nodes)
            {
                foreach (XmlNode Node2 in Node.ChildNodes)
                {
                    if (Node2.Name == key)
                    {
                        returnval = Node2.InnerText;
                    }

                    //if (Node2.Name == "host")
                    //{
                    //    host = Node2.InnerText;
                    //}
                    //if (Node2.Name == "portno")
                    //{
                    //    Port = Node2.InnerText;
                    //}
                    //if (Node2.Name == "login")
                    //{
                    //    username = Node2.InnerText;
                    //}
                    //if (Node2.Name == "password")
                    //{
                    //    password = Node2.InnerText;
                    //}
                    //if (Node2.Name == "clientname")
                    //{
                    //    clientname = Node2.InnerText;
                    //}
                    //if (Node2.Name == "clienturl")
                    //{
                    //    clienturl = Node2.InnerText;
                    //}
                    //if (Node2.Name == "header")
                    //{
                    //    header = Node2.InnerText;
                    //}
                    //if (Node2.Name == "footer")
                    //{
                    //    footer = Node2.InnerText;
                    //}


                }
            }
            //*******Code to get SMS Template

            return returnval;

        }
    }

    
    
}
