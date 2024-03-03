using CsvHelper;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class EmailNotification : CompanySettings, IEmailNotification
    {
        public bool Notify(string Subject, string MessageBody, IEnumerable<Dictionary<string, object>> AttachmentData, string AttachmentFileName, ILogger log)
        {

            // Gmail SMTP server configuration
            string smtpServer = config.SMTP_Server;                 //"smtp.gmail.com";
            int smtpPort = config.SMTP_Port;                        //587;
            string senderEmail = config.SMTP_SenderEmail;           //"allanoidzjr@gmail.com";
            string senderPassword = config.SMTP_SenderPassword;     //"anylytuwpuwkhzho";
            string receiverEmail = config.SMTP_Receivers;           //"allanrobertjaranilla@gmail.com";

            string fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_{AttachmentFileName}.csv";

            // Create a new email message
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = Subject;
            mail.Body = MessageBody;
            mail.CC.Add(config.SMTP_CC);

            var csvBytes = Encoding.UTF8.GetBytes(GenerateCsvData(AttachmentData));

            // Create a memory stream from the byte array
            using (MemoryStream ms = new MemoryStream(csvBytes))
            {
                // Attach the memory stream as a CSV file
                Attachment attachment = new Attachment(ms, fileName, "text/csv");

                // Add the attachment to the email
                mail.Attachments.Add(attachment);

                // Create an SMTP client to send the email
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;


                try
                {
                    // Send the email
                    smtpClient.Send(mail);
                    log.LogInformation($"Email sent successfully. With attachment {fileName}.");
                    return true;
                }
                catch (Exception ex)
                {
                    log.LogError($"An error occurred while sending the email with attachment {fileName}: " + ex.Message);
                    return false;
                }
                finally
                {
                    // Clean up the resources
                    mail.Dispose();
                    attachment.Dispose();
                }
            }
        }

        public bool NotifyWithoutAttachment(string Subject, string MessageBody, ILogger log)
        {

            // Gmail SMTP server configuration
            string smtpServer = config.SMTP_Server;                 //"smtp.gmail.com";
            int smtpPort = config.SMTP_Port;                        //587;
            string senderEmail = config.SMTP_SenderEmail;           //"allanoidzjr@gmail.com";
            string senderPassword = config.SMTP_SenderPassword;     //"anylytuwpuwkhzho";
            string receiverEmail = config.SMTP_Receivers;           //"allanrobertjaranilla@gmail.com";

            // Create a new email message
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = Subject;
            mail.Body = MessageBody;
            mail.CC.Add(config.SMTP_CC);

            // Create an SMTP client to send the email
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Send the email
                smtpClient.Send(mail);
                log.LogInformation($"Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred while sending the email: " + ex.Message);
                return false;
            }
            finally
            {
                // Clean up the resources
                mail.Dispose();
            }
        }

        public bool NotifyWithAttachments(string Subject, string MessageBody, IEnumerable<IEnumerable<Dictionary<string, object>>> AttachmentDataCollection, IEnumerable<string> AttachmentFileNames, ILogger log)
        {
            // Gmail SMTP server configuration
            string smtpServer = config.SMTP_Server;
            int smtpPort = config.SMTP_Port;
            string senderEmail = config.SMTP_SenderEmail;
            string senderPassword = config.SMTP_SenderPassword;
            string receiverEmail = config.SMTP_Receivers;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            // Create a new email message
            MailMessage mail = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = Subject,
                Body = MessageBody
            };

            // Kation email group
            if (!string.IsNullOrEmpty(config.SMTP_CC))
            {
                mail.CC.Add(config.SMTP_CC);
            }

            // Cyware email group
            if (!string.IsNullOrEmpty(config.SMTP_Cyware))
            {
                mail.CC.Add(config.SMTP_Cyware);
            }

            // Loop through each set of AttachmentData and corresponding AttachmentFileName
            for (int i = 0; i < AttachmentDataCollection.Count(); i++)
            {
                string fileName = $"{DateTime.Now.ToString("yyyyMMdd")}_{AttachmentFileNames.ElementAtOrDefault(i)}.csv";

                // Get the current set of AttachmentData
                IEnumerable<Dictionary<string, object>> attachmentData = AttachmentDataCollection.ElementAt(i);

                byte[] csvBytes = Encoding.UTF8.GetBytes(GenerateCsvData(attachmentData));

                MemoryStream stream = new MemoryStream(csvBytes);
                stream.Seek(0, SeekOrigin.Begin);

                // Attach the MemoryStream as an attachment to the email
                Attachment attachment = new Attachment(stream, fileName, "text/csv");
                mail.Attachments.Add(attachment);
            }

            try
            {
                // Send the email
                smtpClient.Send(mail);
                log.LogInformation($"Email sent successfully with {AttachmentDataCollection.Count()} attachment(s).");
                return true;
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred while sending the email: {ex.Message}");
                return false;
            }
            finally
            {
                // Clean up the resources
                mail.Dispose();
                smtpClient.Dispose();
            }
        }


        //private void CreateExcelWorksheet(ExcelPackage package, string worksheetName, IEnumerable<ExpandoObject> data, IEnumerable<ExpandoObject> errorData = null)
        //{
        //    var worksheet = package.Workbook.Worksheets.Add(worksheetName);
        //    var errorCell = "A1";
        //    var dataCell = "B1";

        //    if (errorData != null)
        //    {
        //        worksheet.Cells[errorCell].LoadFromDictionaries(errorData, c =>
        //        {
        //            // Print headers using the property names
        //            c.PrintHeaders = true;
        //            // Insert a space before each capital letter in the header
        //            c.HeaderParsingType = HeaderParsingTypes.CamelCaseToSpace;
        //            // When TableStyle is not TableStyles.None, the data will be loaded into a table with the selected style.
        //            c.TableStyle = TableStyles.Medium24;
        //        });
        //    }
        //    else
        //    {
        //        dataCell = errorCell;
        //    }

        //    worksheet.Cells[dataCell].LoadFromDictionaries(data, c =>
        //    {
        //        // Print headers using the property names
        //        c.PrintHeaders = true;
        //        // When TableStyle is not TableStyles.None, the data will be loaded into a table with the selected style.
        //        c.TableStyle = TableStyles.Medium23;
        //    });

        //    worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].AutoFitColumns();
        //}

        public string GenerateCsvData(IEnumerable<Dictionary<string, object>> dataList)
        {
            dynamic myObject = new ExpandoObject();
            var dynamicList = new List<dynamic>();

            foreach (var dictionary in dataList)
            {
                var expandoDict = (IDictionary<string, object>)new ExpandoObject();

                foreach (var kvp in dictionary)
                {
                    expandoDict.Add(kvp.Key, kvp.Value);
                }

                dynamicList.Add(expandoDict);
            }

            myObject = dynamicList;

            using (var csvWriter = new StringWriter())
            using (var csv = new CsvWriter(csvWriter, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(myObject);
                return csvWriter.ToString();
            }

        }
    }
}
