using Assignment4;
using ClassLibrary;
using ORM;
using System.ComponentModel.DataAnnotations.Schema;


/****  Create Instance for Insert Data  ***
 
Course course = new Course();
course.Id = Guid.NewGuid();
course.Title = "ASP.NET";
course.Fees = 30000;
course.Topics = new List<Topic>();
course.Tests = new List<AdmissionTest>();

Instructor instructor = new Instructor();
instructor.Id = Guid.NewGuid();
instructor.Name = "Jalal Uddin";
instructor.Email = "abcd@gmail.com";
instructor.PhoneNumbers = new List<Phone>();

Address presentAddress = new Address();
presentAddress.Id = Guid.NewGuid();
presentAddress.Street = "hobe ekta";
presentAddress.City = "Dhaka";
presentAddress.Country = "Bangladesh";
presentAddress.AddressType = "Present Address";
Address permanentAddress = new Address();
permanentAddress.Id = Guid.NewGuid();
permanentAddress.Street = "ager ta";
permanentAddress.City = "Noakhali";
permanentAddress.Country = "Bangladesh";
permanentAddress.AddressType = "Permanent Address";

instructor.PresentAddress = presentAddress;
instructor.PermanentAddress = permanentAddress;

Phone phone = new Phone();
phone.Id = Guid.NewGuid();
phone.Number = "987654321";
phone.Extension = "BD";
phone.CountryCode = "+880";
instructor.PhoneNumbers.Add(phone);

course.Teacher = instructor;

Topic topic = new Topic();
topic.Id = Guid.NewGuid();
topic.Title = "Entity FrameWork";
topic.Description = "Work with Database";

topic.Sessions = new List<Session>();
Session session = new Session();
session.Id = 2;
session.DurationInHour = 2;
session.LearningObjective = "Has to be fluend in entity framework";
topic.Sessions.Add(session);

course.Topics.Add(topic);

AdmissionTest admissiontest = new AdmissionTest();
admissiontest.Id = Guid.NewGuid();
admissiontest.StartDateTime = DateTime.Now;
admissiontest.EndDateTime = DateTime.Now;
admissiontest.TestFees = 100.00;

course.Tests.Add(admissiontest);

var insertOrm = new MyORM<Guid, Course>();
insertOrm.Insert(course);

**/

//-----------------------------------------------//



/**Create object for delete***
 
Course course1 = new Course();
course1.Id = new Guid("a1a78306-128d-46e4-bef1-8b006d64af47"); // Have to set id of existing record from database
var deleteOrm = new MyORM<Guid, Course>();
//deleteOrm.Update(course1); // delete by passing object
//deleteOrm.Delete(course1.Id); // delete by passing id

**/

//-----------------------------------------------//


/**Updata data of existing record ***
 
Phone newPhone = new Phone();
newPhone.Id = new Guid("b6248754-589d-4911-9fb7-3884fb3d4f01"); // Have to set id of existing record from database
newPhone.Number = "0162098978";
newPhone.Extension = "Bangladesh";
newPhone.CountryCode = "+880";

var updateOrm = new MyORM<Guid, Phone>();
updateOrm.Update(newPhone);

**/

/* Note: If you set any random Id, the Update method will run
but there will be no change in the data table */

//-----------------------------------------------//

/*Get all relational data of an Id **
  
var course2=new Course();
course2.Id = new Guid("a1a78306-128d-46e4-bef1-8b006d64af47"); // Have to set id of existing record from database

var GetOrm = new MyORM<Guid, Course>();
GetOrm.GetById(course2.Id);

**/

//-----------------------------------------------//

/**Get All Data of a table *

var Orm = new MyORM<Guid, Course>();
Orm.GetAll();

**/
//-----------------------------------------------//

