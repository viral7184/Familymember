using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using familymember.Controllers;
using familymember.Models;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace familymember.Controllers
{
    public class HomeController : Controller
    {
        Database1Entities db = new Database1Entities();
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult practise()
        {
            return View();
        }




        public ActionResult treenode()
        {
            //List<tbl_member> all = new List<tbl_member>();
            //using(Database1Entities dc=new Database1Entities())
            //{
           
            var all =db.tbl_member.ToList();
            ViewBag.tree = all;
            return View();
        }

        public JsonResult Getsubmenu(string pid)
        {
            List<tbl_member> all = new List<tbl_member>();
            int pID = 0;
            int.TryParse(pid, out pID);
            using (Database1Entities dc = new Database1Entities())
            {
                all = dc.tbl_member.Where(a => a.FIRSTNAME.Equals(0)).ToList();
            }
            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Registration()
        {
            return View();
        }   

        [HttpPost]
        public ActionResult Registration(tbl_member data, FormCollection frm)
        {
            data.CREATED_DATE = DateTime.Now;
            int OTP = Convert.ToInt32(Request.Form["OTP"]);
            int otpnumber = Convert.ToInt32(Session["OTP"]);
            if (otpnumber == OTP)
            {
                data.IS_DELETED = false;
                db.tbl_member.Add(data);
            }
            else
            {
                Session["otpnumber"] = "OTP does Not match";
                return RedirectToAction("Registration");
            }

            var member_roll = db.tbl_member_role.Where(i => i.MEMBER_ID == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();

            if (member_roll != null)
            {

            }
            else
            {
                var employee_role = new tbl_member_role();
                if (employee_role != null)
                {
                    employee_role.MEMBER_ID = data.Id;
                    employee_role.MEMBER_ROLE = "MainMember";
                    employee_role.CREATED_DATE = DateTime.Now;
                    db.tbl_member_role.Add(employee_role);
                    db.SaveChanges();
                    Session["Mainmember_ID"] = data.Id;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Mainmember");
        }

        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_member data)
        {
            try
            {
                var users = db.tbl_member.Where(i => i.EMAIL == data.EMAIL && i.PASSWORD == data.PASSWORD).FirstOrDefault();
                Session["user"] = users;           
                if (users == null)
                {
                    TempData["msg"] = "Email id Or Password is wrong";
                    return RedirectToAction("Login");
                }
                else
                {
                    var roll = users.tbl_member_role.FirstOrDefault();
                    Session["Id"] = users.Id;
                    Session["name"] = users.FIRSTNAME.Trim();
                    Session["roll"] = roll.MEMBER_ROLE;
                    Session["email"] = users.EMAIL;
                   
                    grpnoti(users.FIRSTNAME);
                    return RedirectToAction("Familymember");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string changepassword(tbl_member data)
        {
            try
            {
                //var email = Request.Form["EMAIL"];
                //var pass = Request.Form["PASSWORD"];

                var memeber = db.tbl_member.Where(i => i.EMAIL == data.EMAIL).FirstOrDefault();
                if (memeber != null)
                {
                    memeber.PASSWORD = data.PASSWORD;
                    db.SaveChanges();
                    return "Your password has been successfully changed!";
                }
                return "Please check inserted data.";
            }
            catch (Exception e)
            {
                return "Something went wrong! Please try again.";
            }
        }

        public ActionResult OTP(string email)
        {
            try
            {
                // var email1 = db.tbl_member.Where(i => i.EMAIL == email).FirstOrDefault();
                if (email != null)
                {
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    int RandomNumber;
                    RandomNumber = rand.Next(100000, 999999);
                    var passtext = RandomNumber;
                    Session["OTP"] = passtext;
                    sendmail(email, passtext);
                    Session["check_email"] = "check your Email";
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("login");
        }

        public ActionResult forgotpasswrd(FormCollection frm)
        {
            try
            {
                var email = Request.Form["EMAIL"];
                if (email != null)
                {
                    var user = db.tbl_member.Where(i => i.EMAIL == email).FirstOrDefault();
                    if (user != null)
                    {
                        var passtext = user.PASSWORD;
                        Session["pass"] = passtext;
                        sendpassword(email, passtext);
                        Session["check_email"] = "check your Email"; 
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("login");
        }

            public  void grpnoti(string name)
            {
            try
            {                
                var noti = db.tbl_notification.Where(i => i.MEMBER_NAME == name && i.IS_NOTIFY == false).ToList();
                ViewBag.groupname = noti;
                Session["noticount"] = noti.Count;
                ViewData["groupname"] = noti;

                var notifctn = "";
                if (noti != null)
                {
                    foreach (var grp in noti)
                    {
                        notifctn = notifctn + "<div class='form-group'> <label> Are You sure to join " + grp.GROUP_NAME + " group </label>" +
                            "<button type = 'submit' onclick =groupnotification('accept','" + grp.GROUP_NAME + "') class='btn btn-info pull-right'>Accept</button>" +
                            "<button type = 'submit' onclick =groupnotification('denied','" + grp.GROUP_NAME + "') class='btn btn-info pull-right'>Denied</button></div>";
                    }
                }
                Session["groupname"] = notifctn;

            }
            catch (Exception e)
            {

               // return "there is some error plz try again!" + e;
            }

        }

        //public void grpjoined(string group_noti, tbl_notification data)
        //{
        //    var membername = Session["name"].ToString();
        //    var groupname = data.GROUP_NAME;
        //    if (group_noti == "accept")
        //    {
        //        var a = db.tbl_notification.Where(i => i.MEMBER_NAME == membername && i.GROUP_NAME == groupname).SingleOrDefault();
        //        a.IS_NOTIFY = true;
        //        db.SaveChanges();
        //    }

        //    if (group_noti == "denied")
        //    {
        //        var a = db.tbl_notification.Where(i => i.MEMBER_NAME == membername && i.GROUP_NAME == groupname).SingleOrDefault();
        //        a.IS_NOTIFY = false;
        //        db.SaveChanges();
        //    }
        //    Session["groupname"] = "";
            
        //}

        public static string sendpassword(string email, string pass)
        {
            try
            {
                var frmemail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];
                var frmpass = System.Configuration.ConfigurationManager.AppSettings["EMPass"];
                var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"]);
                var fromAddress = new MailAddress(frmemail, "Family Member");
                var toAddress = new MailAddress(email);
                string password = pass;
                string fromPassword = frmpass;
                string subject = "well come to procorner";
                string body = "your password is :" + password + "";
                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                //var message = new MailMessage(fromAddress, toAddress);
                //message.IsBodyHtml = true;
                //message.Subject = subject;
                //message.Body = body;
                //  message.IsBodyHtml = true;
                {

                    smtp.Send(message);
                }

                return "email send successfully to" + toAddress + "!";

            }
            catch (Exception e)
            {

                return "there is some error plz try again!" + e;
            }
        }
        
        public static string sendmail(string email, int OTP)
        {
            try
            {
                var frmemail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"];
                var frmpass = System.Configuration.ConfigurationManager.AppSettings["EMPass"];
                var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"]);
                var fromAddress = new MailAddress(frmemail, "Family Member");
                var toAddress = new MailAddress(email);

                string fromPassword = frmpass;

                string otp = OTP.ToString();

                string subject = "OTP Number";
                string body = "your OTP is :" + otp;
                var smtp = new SmtpClient
                {   
                    Host = host,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                //var message = new MailMessage(fromAddress, toAddress);
                //message.IsBodyHtml = true;
                //message.Subject = subject;
                //message.Body = body;
                //  message.IsBodyHtml = true;
                {

                    smtp.Send(message);
                }

                return "Email send successfully to" + toAddress + "!";

            }
            catch (Exception e)
            {

                return "There is some error, Please try again!" + e;
            }
        }

        public ActionResult logout()
        {
            try
            {
                Session.Abandon();
                Session.Clear();
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Login");
        }

        public ActionResult Relationship()
        {
            //var loginid = Convert.ToInt16(Session["Id"]);
            var relation = db.tbl_relationship.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
            ViewBag.relationship = relation;
            return View();
        }
        [HttpPost]
        public ActionResult Relationship(tbl_relationship data)
        {
            try
            {
                if (data.Id > 0)
                {
                    var relation = db.tbl_relationship.Where(i => i.Id == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();
                    if (relation != null)
                    {

                        if (data.Id > 0)
                        {
                            relation.RELATIONSHIP = data.RELATIONSHIP;
                        }

                        db.SaveChanges();
                    }

                }
                else
                {
                    data.IS_DELETED = false;
                    //data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_relationship.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return RedirectToAction("Relationship");
        }

        public JsonResult getrelation(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var relation = db.tbl_relationship.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = relation }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public ActionResult profile(int id)
        {
            var emp = db.tbl_member.Where(i => i.Id == id && i.IS_DELETED.Value.Equals(false)).FirstOrDefault();
            return View(emp);
        }


        [HttpPost]
        public ActionResult profile(tbl_member data)
        {
            try
            {
                if (data.Id > 0)
                {
                    var events = db.tbl_member.Where(i => i.Id == data.Id).SingleOrDefault();
                    if (data.FIRSTNAME != null)
                    {
                        events.FIRSTNAME = data.FIRSTNAME;
                    }
                    if (data.SURNAME != null)
                    {
                        events.SURNAME = data.SURNAME;
                    }

                    if (data.EMAIL != null)
                    {
                        events.EMAIL = data.EMAIL;
                    }
                    if (data.MOBILE1 != null)
                    {
                        events.MOBILE1 = data.MOBILE1;
                    }
                    if (data.MOBILE2 != null)
                    {
                        events.MOBILE2 = data.MOBILE2;
                    }
                    if (data.DOB != null)
                    {
                        events.DOB = data.DOB;
                    }
                    if (data.BLOODGROUP != null)
                    {
                        events.BLOODGROUP = data.BLOODGROUP;
                    }
                    if (data.EDUCATION != null)
                    {
                        events.EDUCATION = data.EDUCATION;
                    }
                    events.MODIFIED_DATE = DateTime.Now;
                    db.SaveChanges();

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("profile");
        }

        public ActionResult tree()
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            var ab = db.tbl_member/*.Where(i=>i.LOGIN_BY == loginby)*/.ToList();
            ViewBag.tree=ab;
            return View(ab);
        }

        public ActionResult Mainmember(int? id)
        {
            var relation = db.tbl_relationship.ToList();
            ViewBag.relationship = relation;
            var loginby = Convert.ToInt16(Session["Id"]);
            var date = DateTime.Now.Date;
            var events = db.tbl_events.Where(i => i.EVENTDATE == date && i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
            Session["events"] = events.Count();

            if (loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;
            }
            else
            {
                if (id > 0)
                {
                    var abc = db.tbl_member.Where(i => i.Id == id && i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member = abc;
                }
                else
                {
                    var tbl4 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member = tbl4;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Mainmember(tbl_member data)
        {
            try
            {
                if (data.Id > 0)
                {
                    var events = db.tbl_member.Where(i => i.Id == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();
                    if (data.FIRSTNAME != null)
                    {
                        events.FIRSTNAME = data.FIRSTNAME;
                    }
                    if (data.SURNAME != null)
                    {
                        events.SURNAME = data.SURNAME;
                    }
                    if (data.EMAIL != null)
                    {
                        events.EMAIL = data.EMAIL;
                    }
                    if (data.MOBILE1 != null)
                    {
                        events.MOBILE1 = data.MOBILE1;
                    }
                    if (data.MOBILE2 != null)
                    {
                        events.MOBILE2 = data.MOBILE2;
                    }
                    if (data.DOB != null)
                    {
                        events.DOB = data.DOB;
                    }
                    if (data.BLOODGROUP != null)
                    {
                        events.BLOODGROUP = data.BLOODGROUP;
                    }
                    if (data.EDUCATION != null)
                    {
                        events.EDUCATION = data.EDUCATION;
                    }
                    events.MODIFIED_DATE = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    data.CREATED_DATE = DateTime.Now;
                    data.IS_ACTIVE = true;
                    data.IS_DELETED = false;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);

                    db.tbl_member.Add(data);
                    db.SaveChanges();

                    var member_roll = db.tbl_member_role.Where(i => i.MEMBER_ID == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();

                    if (member_roll != null)
                    {

                    }
                    else
                    {
                        var employee_role = new tbl_member_role();
                        if (employee_role != null)
                        {
                            employee_role.MEMBER_ID = data.Id;
                            employee_role.MEMBER_ROLE = "MainMember";
                            employee_role.CREATED_DATE = DateTime.Now;
                            db.tbl_member_role.Add(employee_role);
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Mainmember");
        }
        

        public JsonResult getmember(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var member = db.tbl_member.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = member }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public ActionResult Familymember(int? id)
          {
            var relation = db.tbl_relationship.ToList();
            ViewBag.relationship = relation;

            var member1 = db.tbl_member/*.Select(i => i.FIRSTNAME)*/.ToList();
            ViewBag.member = member1;

            var date = DateTime.Now.Date;
            var loginby = Convert.ToInt16(Session["Id"]);
            var events = db.tbl_events.Where(i => i.EVENTDATE == date && i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
            Session["events"] = events.Count();

          //  var loginby = Convert.ToInt16(Session["Id"]);
          
            
            if (loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;
            }
            else
            {
                if (id > 0)
                {
                    if (loginby == 0)
                    {
                        var abc = db.tbl_member.Where(i => i.Id == id && i.IS_DELETED.Value.Equals(false)).ToList();
                        ViewBag.member = abc;
                    }
                    else
                    {
                        var abc = db.tbl_member.Where(i => i.Id == id && i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                        ViewBag.member = abc;
                    }
                }
                else
                {
                    if (loginby == 0)
                    {
                        var abc = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                        ViewBag.member = abc;
                    }
                    else
                    {
                        var tbl4 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                        ViewBag.member = tbl4;
                    }
                }
            }
          
            return View();            
        }
               
        [HttpPost]
        public ActionResult Familymember(tbl_member data)
            {
            try
            {
                if (data.Id > 0)
                {
                    var events = db.tbl_member.Where(i => i.Id == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();
                    if (data.FIRSTNAME != null)
                    {
                        events.FIRSTNAME = data.FIRSTNAME;
                    }
                    if (data.SURNAME != null)
                    {
                        events.SURNAME = data.SURNAME;
                    }

                    if (data.EMAIL != null)
                    {
                        events.EMAIL = data.EMAIL;
                    }
                    if (data.MOBILE1 != null)
                    {
                        events.MOBILE1 = data.MOBILE1;
                    }
                    if (data.MOBILE2 != null)
                    {
                        events.MOBILE2 = data.MOBILE2;
                    }
                    if (data.DOB != null)
                    {
                        events.DOB = data.DOB;
                    }
                    if (data.BLOODGROUP != null)
                    {
                        events.BLOODGROUP = data.BLOODGROUP;
                    }
                    if (data.EDUCATION != null)
                    {
                        events.EDUCATION = data.EDUCATION;
                    }
                    events.MODIFIED_DATE = DateTime.Now;
                    db.SaveChanges();

                }
                else
                {
                    data.CREATED_DATE = DateTime.Now;
                    data.IS_ACTIVE = true;
                    data.IS_DELETED = false;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_member.Add(data);
                    db.SaveChanges();

                    var member_roll = db.tbl_member_role.Where(i => i.MEMBER_ID == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();
                    if (member_roll != null)
                    {

                    }
                    else
                    {
                        var employee_role = new tbl_member_role();
                        if (employee_role != null)
                        {
                            employee_role.MEMBER_ID = data.Id;
                            employee_role.MEMBER_ROLE = "SubMember1";
                            employee_role.CREATED_DATE = DateTime.Now;
                            db.tbl_member_role.Add(employee_role);
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Familymember");
        }
         
                
        //public ActionResult creategroup(tbl_group data,FormCollection frm)
        //{
        //    string[] field = frm.GetValues("MEMBER");
        //    List<tbl_group> allData = new List<tbl_group>();
        //    string intfield = "";
        //    for (var i = 0; i < field.Length; i++)
        //    {
        //        var item = field[i];
        //        if (i != 0 && i != field.Length)
        //        {
        //            intfield += ",";
        //        }
        //        intfield += item;
        //    }

        //    data.CREATED_DATE = DateTime.Now;
        //    data.IS_ACTIVE = true;
        //    data.IS_DELETED = false;
        //    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
        //    data.ADMIN = data.LOGIN_BY;
        //    data.MEMBER = intfield;
        //    var group = db.tbl_group.Add(data);
        //    db.SaveChanges();
        //    return Redirect("creategroup");
        //}
        
        public ActionResult groupuser()
        {            
            var member1 = db.tbl_member.ToList();
            ViewBag.member4 = member1;
            var group = db.tbl_groupuser.Where(i => i.IS_DELETED.Value.Equals(false));
            ViewBag.group3 = group;          
            return View();
        }
        [HttpPost]
        public ActionResult groupuseredit(tbl_notification data)
        {
            var ab = db.tbl_notification.Where(i => i.Id == data.Id).SingleOrDefault();
            if(ab.GROUP_NAME != data.GROUP_NAME)
            {
                var user = db.tbl_notification.Where(i => i.GROUP_NAME == ab.GROUP_NAME).ToList();
                ViewBag.noti = user;
                user.ForEach(i => i.GROUP_NAME = data.GROUP_NAME);                
                db.SaveChanges();
            }         
            else
            { 
                if(data.MEMBER_NAME != null)
                { 
                  data.ADMIN = "No";
                  db.tbl_notification.Add(data);
                  db.SaveChanges();
                }
            }
            return RedirectToAction("notification");
        }

        public JsonResult loadmemlist2(int id)
        {
            var user = db.tbl_notification.Where(i => i.Id == id).SingleOrDefault();

            var group = user.GROUP_NAME.ToString();
           
            var ab1 = db.tbl_notification.Where(i => i.GROUP_NAME == group).ToList();
           
            var a = db.tbl_member.ToList();
            foreach (var item in ab1)
            {
                a = a.Where(i => !i.FIRSTNAME.Contains(item.MEMBER_NAME.ToString())).ToList();
                ViewBag.memlist = a;
            }
            var c = a.Select(i => new
            {
                ID = i.Id,                
                MEMBERLIST = i.FIRSTNAME,
            }).ToList();

            return Json(new { data = c }, JsonRequestBehavior.AllowGet);

        }
 
        //public JsonResult loadmemlist(int id)
        //{           
        //    var user = db.tbl_groupuser.Where(i => i.Id == id).SingleOrDefault();
        //    var member = user.MEMBER_NAME;
        //    var ab1 = member.Split(',');
        
        //    var a = db.tbl_member.ToList();
        //    foreach (var item in ab1)
        //    {
        //        a = a.Where(i => !i.FIRSTNAME.Contains(item)).ToList();
        //        ViewBag.memlist = a;
        //    }
        //    var c = a.Select(i => new
        //    {
        //        ID=i.Id,
        //        MEMBERLIST=i.FIRSTNAME,                
        //    }).ToList();

        //    return Json(new { data = c }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public ActionResult groupuser(tbl_groupuser data, tbl_notification noti, FormCollection frm)
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            string[] field = frm.GetValues("MEMBER_NAME");
            
            List<tbl_groupuser> allData = new List<tbl_groupuser>();
            string intfield = "";

            for (var i = 0; i < field.Length; i++)
            {
                var item = field[i];
                if (i != 0 && i != field.Length)
                {
                    intfield += ",";
                }
                intfield += item;
            }
            var a = db.tbl_groupuser.Where(i => !intfield.Contains(i.MEMBER_NAME));
            ViewBag.listmember = a;

            data.ISACTIVE = true;
            data.IS_DELETED = false;
            data.USER_ADDED_DATE = DateTime.Now;
            data.LOGIN_BY = loginby;
            data.MEMBER_NAME = intfield + "," + Session["name"];
            //data.MEMBER_NAME = Session["name"].ToString();
            var group = db.tbl_groupuser.Add(data);
            db.SaveChanges();

            //var s = "";
            //for (int i = 0; i < field[i].Length; i++)
            //{
            //    s = intfield[i].ToString();

            //    var mber = db.tbl_notification.Where(j => j.MEMBER_NAME == field[i]).SingleOrDefault();
            //    mber.MEMBER_NAME = field[i];
            //    mber.IS_NOTIFY = false;
            //    db.SaveChanges();
            //}

            foreach (string item in field)
            {
                string i = "";      
                //noti.MESSAGE =Session["groupname"].ToString();    
                noti.MEMBER_NAME = item.ToString();
                noti.IS_DELETED = false;
                noti.ADMIN = "No";
                noti.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                //noti.MEMBER_NAME = Session["name"].ToString();
                var mber = db.tbl_notification.Add(noti);
                mber.IS_NOTIFY = false;
                db.SaveChanges();
            }
            noti.MEMBER_NAME = Session["name"].ToString();
            noti.IS_DELETED = false;
            noti.ADMIN = "Yes";
            //noti.LOGIN_BY = Convert.ToInt16(Session["Id"]);
            //noti.MEMBER_NAME = Session["name"].ToString();
            var mber2 = db.tbl_notification.Add(noti);
            mber2.IS_NOTIFY = true;
            db.SaveChanges();
            return Redirect("notification");
        }


        public JsonResult getgroupuser(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var events = db.tbl_groupuser.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = events }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public ActionResult groupadmin()
        {
            var group = db.tbl_groupadmin.Where(i => i.IS_DELETE.Value.Equals(false)/* && i.LOGIN_BY == loginby*/);
            ViewBag.group2 = group;
            return View();
        }

        [HttpPost]
        public ActionResult groupadmin(tbl_groupadmin data)
        {
            data.IS_ACTIVE = true;
            data.IS_DELETE = false;
            data.CREATED_DATE = DateTime.Now;

            var group = db.tbl_groupadmin.Add(data);
            db.SaveChanges();
            return Redirect("groupadmin");
        }

        public ActionResult member_role()
        {
            var member = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
            ViewBag.memberlist = member;

            var memberole = db.tbl_member_role.OrderByDescending(i => i.Id).ToList();
            ViewBag.member_role = memberole;

            return View();
        }

        [HttpPost]
        public ActionResult member_role(tbl_member_role data)
        {
            try
            {
                if (data.Id > 0)
                {
                    var emp_roll_master = db.tbl_member_role.Where(i => i.Id == data.Id && i.IS_DELETED.Value.Equals(false)).SingleOrDefault();
                    if (emp_roll_master != null)
                    {
                        emp_roll_master.MODIFIED_DATE = DateTime.Now;
                        if (data.MEMBER_ID > 0)
                        {
                            emp_roll_master.MEMBER_ID = data.MEMBER_ID;
                        }
                        if (data != null)
                        {
                            emp_roll_master.MEMBER_ROLE = data.MEMBER_ROLE;
                        }
                        db.SaveChanges();
                    }

                }
                else
                {
                    data.CREATED_DATE = DateTime.Now;
                    data.IS_DELETED = false;
                    db.tbl_member_role.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("member_role");
        }

        public JsonResult getmemberrole(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var emprole = db.tbl_member_role.Where(i => i.MEMBER_ID == id).SingleOrDefault();
                return Json(new { data = emprole }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }



        public ActionResult memberdetails(int id)
        {
            var emp = db.tbl_member.Where(i => i.Id == id && i.IS_DELETED.Value.Equals(false)).FirstOrDefault();
            return View(emp);
        }


        public ActionResult FamilyDetails()
        {
            return View();
        }

        public ActionResult Surname()
        {
            var tbl4 = db.tbl_surname.Where(i => i.IS_DELETED.Equals(false)).ToList();
            ViewBag.surn = tbl4;
            return View();
        }

        [HttpPost]
        public ActionResult Surname(tbl_surname data)
        {
            try
            {
                if (data.Id > 0)
                {
                    var loginby = Convert.ToInt16(Session["Id"]);
                    var surname = db.tbl_surname.Where(i => i.Id == data.Id && i.LOGIN_BY == loginby).SingleOrDefault();
                    surname.SURNAME = data.SURNAME;

                    db.SaveChanges();

                }
                else
                {
                    data.IS_ACTIVE = true;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_surname.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Surname");
        }

        public JsonResult getsurname(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var surname = db.tbl_surname.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = surname }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }



        public ActionResult ImageGallery()
        {
            
            var loginby = Convert.ToInt16(Session["Id"]);
            if(loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;
            }
            else
            { 
            if (loginby == 0)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;
            }
            else
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                ViewBag.img = tbl2;
            }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ImageGallery(tbl_images data, HttpPostedFileBase[] files)
        {
            try
            {
                var f_no = db.tbl_images.Where(i => i.Id == data.Id).FirstOrDefault();

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        file.SaveAs(path);
                        data.IMAGE = fileName;
                         db.SaveChanges();
                    }
                }
                //if (data.IMAGE != null)
                //{
                //    Models.tbl_images imgs = new tbl_images();
                //    imgs.IMAGE = data.IMAGE;
                //    HttpFileCollectionBase file = Request.Files;
                //    for (int i = 0; i < file.Count; i++)
                //    {
                //        HttpPostedFileBase Image = file[i];
                //        var filename = "";
                //        if (Image != null)
                //        {
                //            filename = Path.GetFileName(Image.FileName);
                //            var path = Path.Combine(Server.MapPath("~/images"), filename);

                //            Image.SaveAs(path);
                //            data.IMAGE = filename;
                //            db.SaveChanges();
                //        }
                //    }
                //}
                data.IS_DELETED = false;
                data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                db.tbl_images.Add(data);

                db.SaveChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("ImageGallery");
        }


        public ActionResult VideoGallery()
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            if (loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;

                var video1 = db.tbl_videos.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.video = video1;
            }
            else
            {
                if (loginby == 0)
                {
                    var video1 = db.tbl_videos.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.video = video1;
                }
                else
                {
                    var video1 = db.tbl_videos.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.video = video1;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult VideoGallery(tbl_videos data, HttpPostedFileBase[] files)
        {
            try
            {
                var f_no = db.tbl_images.Where(i => i.Id == data.Id).FirstOrDefault();

                if (data.VIDEO != null)
                {
                    Models.tbl_images imgs = new tbl_images();
                    imgs.IMAGE = data.VIDEO;
                    HttpFileCollectionBase file = Request.Files;
                    for (int i = 0; i < file.Count; i++)
                    {
                        HttpPostedFileBase Image = file[i];
                        var filename = "";
                        if (Image != null)
                        {
                            filename = Path.GetFileName(Image.FileName);
                            var path = Path.Combine(Server.MapPath("~/video"), filename);

                            Image.SaveAs(path);
                            data.VIDEO = filename;
                            db.SaveChanges();
                        }
                    }
                }
                data.IS_DELETED = false;
                data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                db.tbl_videos.Add(data);

                db.SaveChanges();

            }
            catch (Exception e)
            {

                throw e;
            }


            return RedirectToAction("VideoGallery");
        }
        
                        
        public ActionResult Events(string eve)
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            if (loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;
                var today1 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                ViewBag.member = today1;
                var date1 = DateTime.Now.Date;
                var today = db.tbl_events.Where(i => i.EVENTDATE == date1 && i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.eve = today;
            }
            else
            {
                if (loginby == 0)
                {
                    if (eve == "event")
                    {
                        var date1 = DateTime.Now.Date;
                        var today = db.tbl_events.Where(i => i.EVENTDATE == date1 && i.IS_DELETED.Value.Equals(false)).ToList();
                        ViewBag.eve = today;

                    }
                    else
                    {
                        var tbl1 = db.tbl_events.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                        ViewBag.eve = tbl1;
                    }
                    var today1 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member = today1;
                }
                else
                {
                    if (eve == "event")
                    {
                        var date1 = DateTime.Now.Date;
                        var today = db.tbl_events.Where(i => i.EVENTDATE == date1 && i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                        ViewBag.eve = today;

                    }
                    else
                    {
                        var tbl1 = db.tbl_events.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                        ViewBag.eve = tbl1;
                    }
                    var today1 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member = today1;
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult Events(tbl_events data)
        {

            try
            {
                if (data.Id > 0)
                {
                    var events = db.tbl_events.Where(i => i.Id == data.Id).SingleOrDefault();
                    events.MEMBER_ID = data.MEMBER_ID;
                    events.EVENTNAME = data.EVENTNAME;
                    events.EVENTDATE = data.EVENTDATE;
                    events.CREATED_BY = data.CREATED_BY;
                    db.SaveChanges();

                }
                else
                {
                    data.IS_ACTIVE = true;
                    data.IS_DELETED = false;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_events.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Events");
        }
        public JsonResult getevent(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var events = db.tbl_events.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = events }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }



        public ActionResult Notification()
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            if (loginby == 2)
            {
                var tbl2 = db.tbl_notification.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;

                var tbl3 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.member2 = tbl3;

                var groupname = from name in db.tbl_notification
                                group name by name.GROUP_NAME into empg
                                select new { NAME = empg.Key, Value = empg };
                               
                Dictionary<string, List<tbl_notification>> intwork = new Dictionary<string, List<tbl_notification>>();

                foreach (var ab in groupname)
                {
                    var group2 = db.tbl_notification.Where(j => j.GROUP_NAME == ab.NAME.ToString()).ToList();
                    intwork.Add(ab.NAME, group2);
                }
                ViewBag.work = intwork;
            }
            else
            {
                if (loginby == 0)
                {
                    var noti = db.tbl_notification.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.notifi = noti;

                    var tbl3 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.member2 = tbl3;

                    var tbl4 = db.tbl_groupuser.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.member3 = tbl4;

                    var groupname = from name in db.tbl_notification
                                    group name by name.GROUP_NAME into empg
                                    select new { NAME = empg.Key, Value = empg };

                    Dictionary<string, List<tbl_notification>> intwork = new Dictionary<string, List<tbl_notification>>();

                    foreach (var ab in groupname)
                    {
                        var group2 = db.tbl_notification.Where(j => j.GROUP_NAME == ab.NAME.ToString()).ToList();
                        intwork.Add(ab.NAME, group2);
                    }
                    ViewBag.work = intwork;
                }
                else
                {
                    var noti = db.tbl_notification.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.notifi = noti;
                    var tbl3 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member2 = tbl3;
                    var tbl4 = db.tbl_groupuser.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member3 = tbl4;
                    var groupname = from name in db.tbl_notification
                                    group name by name.GROUP_NAME into empg
                                    select new { NAME = empg.Key, Value = empg };

                    Dictionary<string, List<tbl_notification>> intwork = new Dictionary<string, List<tbl_notification>>();

                    foreach (var ab in groupname)
                    {
                        var group2 = db.tbl_notification.Where(j => j.GROUP_NAME == ab.NAME.ToString()).ToList();
                        intwork.Add(ab.NAME, group2);
                    }
                    ViewBag.work = intwork;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Notification(tbl_notification data)
        { 
            try
            {
                if (data.Id > 0)
                {
                    var noti = db.tbl_notification.Where(i => i.Id == data.Id).SingleOrDefault();
                    if(data.MEMBER_NAME !=null)
                    { 
                    noti.MEMBER_NAME = data.MEMBER_NAME;
                    }
                    noti.MESSAGE = data.MESSAGE;
                    noti.ADMIN = data.ADMIN;
                    noti.IS_NOTIFY = data.IS_NOTIFY;
                    db.SaveChanges();
                }
                else
                {
                    data.IS_DELETED = false;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_notification.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Notification");
        }

        
        public ActionResult groupnoti(string group_noti,tbl_notification data)
        {

            var membername = Session["name"].ToString();
            var groupname = data.GROUP_NAME;
            if (group_noti == "accept")
            {
                var a = db.tbl_notification.Where(i => i.MEMBER_NAME == membername && i.GROUP_NAME == groupname).SingleOrDefault();
                a.IS_NOTIFY = true;
                db.SaveChanges();
            }

            if (group_noti == "denied")
            {
                var a = db.tbl_notification.Where(i => i.MEMBER_NAME == membername && i.GROUP_NAME == groupname).SingleOrDefault();
                a.IS_NOTIFY = null;
                db.SaveChanges();
            }
            grpnoti(membername);
            return RedirectToAction("Familymember");
        }

        public JsonResult getnotification(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var notification = db.tbl_notification.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = notification }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;    
            }
        }

        public ActionResult Address()
        {
            var loginby = Convert.ToInt16(Session["Id"]);
            if (loginby == 2)
            {
                var tbl2 = db.tbl_images.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.img = tbl2;

                var tbl4 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.member = tbl4;

                var tbl = db.tbl_address.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                ViewBag.addr = tbl;
            }
            else
            {
                if (loginby == 0)
                { 
                    var tbl4 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.member = tbl4;
                }
                else
                {
                    var tbl4 = db.tbl_member.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.member = tbl4;
                }
                if (loginby == 0)
                {
                    var tbl = db.tbl_address.Where(i => i.IS_DELETED.Value.Equals(false)).ToList();
                    ViewBag.addr = tbl;
                }
                else
                {
                    var tbl = db.tbl_address.Where(i => i.IS_DELETED.Value.Equals(false) && i.LOGIN_BY == loginby).ToList();
                    ViewBag.addr = tbl;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Address(tbl_address data)
        {

            try
            {
                if (data.Id > 0)
                {
                    var address = db.tbl_address.Where(i => i.Id == data.Id).SingleOrDefault();
                    address.MEMBER_ID = data.MEMBER_ID;
                    address.STREET = data.STREET;
                    address.CITY = data.CITY;
                    address.STATE = data.STATE;
                    address.PINCODE = data.PINCODE;
                    db.SaveChanges();

                }
                else
                {

                    data.IS_ACTIVE = true;
                    data.IS_DELETED = false;
                    data.LOGIN_BY = Convert.ToInt16(Session["Id"]);
                    db.tbl_address.Add(data);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Address");
        }
        public JsonResult getaddress(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var address = db.tbl_address.Where(i => i.Id == id).SingleOrDefault();
                return Json(new { data = address }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public JsonResult delete(int id, string add_del, string m_del, string a_del, string memrole_del, string n_del, string image_del, string video_del, string relation_del, string group_del)
        {
            if (a_del != null)
            {
                var usefile = db.tbl_events.Where(i => i.Id == id).SingleOrDefault();
                if (usefile != null)
                {
                    bool ab = true;
                    usefile.IS_DELETED = ab;
                    db.SaveChanges();
                }
            }


            else if (add_del != null)
            {
                var filedelete1 = db.tbl_address.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_address.Remove(filedelete1);
                db.SaveChanges();
            }

            else if (m_del != null)
            {
                var member2 = db.tbl_member.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_member.Remove(member2);
                db.SaveChanges();
            }

            else if (memrole_del != null)
            {
                var member2 = db.tbl_member_role.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_member_role.Remove(member2);
                db.SaveChanges();
            }


            else if (n_del != null)
            {
                var noti = db.tbl_notification.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_notification.Remove(noti);
                db.SaveChanges();
            }

            else if (image_del != null)
            {
                var image = db.tbl_images.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_images.Remove(image);
                db.SaveChanges();
            }

            else if (video_del != null)
            {
                var video = db.tbl_videos.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_videos.Remove(video);
                db.SaveChanges();
            }

            else if (relation_del != null)
            {
                var relation = db.tbl_relationship.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_relationship.Remove(relation);
                db.SaveChanges();
            }

            else if (group_del != null)
            {
                var group = db.tbl_groupuser.Where(i => i.Id == id).SingleOrDefault();
                db.tbl_groupuser.Remove(group);
                db.SaveChanges();
            }
            return Json("");
        }

    }
}