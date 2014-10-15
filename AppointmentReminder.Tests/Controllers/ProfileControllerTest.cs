using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentReminder.Tests.Controllers
{
	using System.Collections.Generic;
	using System.Security.Principal;
	using System.Threading;
	using System.Web;
	using System.Web.Mvc;
	using AppointmentReminder.Controllers;
	using AppointmentReminder.Data;
	using AppointmentReminder.Models;
	using Telerik.JustMock;

	[TestClass]
	public class ProfileControllerTest
	{
		[TestInitialize]
		public void Startup()
		{

		}

		[TestMethod]
		public void ProfileController_Edit_Returns_XXXX()
		{

		}

		[TestMethod]
		public void ProfileController_Index_Returns_Profile_In_DB_Not_Null()
		{
			//Arrange
			string userName = "jdoe";
			var mock = Mock.Create<ControllerContext>();
			Mock.Arrange(() => mock.HttpContext.User.Identity.Name).Returns(userName);
			Mock.Arrange(() => mock.HttpContext.Request.IsAuthenticated).Returns(true);
			var profileRepository = Mock.Create<IReminderDb>();
			var profileModel = Mock.Create<IProfileModel>();

			Mock.Arrange(() => profileRepository.GetProfile(userName))
				.Returns(new Profile { Id = 1, FirstName = "John", LastName = "Doe", UserName = userName, PhoneNumber = "7145551212", EmailAddress = "noreply@noreply.com" }).MustBeCalled();

			//Act
			var profileController = new ProfileController(profileRepository, profileModel);
			profileController.ControllerContext = mock;

			ViewResult viewResult = profileController.Index();
			var model = viewResult.Model as IProfileModel;

			//Assert
			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void ProfileController_Index_Returns_Profile_With_All_Correct_Properties()
		{
			//Arrange
			string userName = "jdoe";
			var mock = Mock.Create<ControllerContext>();
			Mock.Arrange(() => mock.HttpContext.User.Identity.Name).Returns(userName);
			Mock.Arrange(() => mock.HttpContext.Request.IsAuthenticated).Returns(true);
			var profileRepository = Mock.Create<IReminderDb>();
			var profileModel = Mock.Create<IProfileModel>();

			Mock.Arrange(() => profileRepository.GetProfile(userName))
				.Returns(new Profile { Id = 1, FirstName = "John", LastName = "Doe", UserName = userName, PhoneNumber = "7145551212", EmailAddress = "noreply@noreply.com" }).MustBeCalled();

			//Act
			var profileController = new ProfileController(profileRepository, profileModel);
			profileController.ControllerContext = mock;

			ViewResult viewResult = profileController.Index();
			var model = viewResult.Model as IProfileModel;

			//Assert
			Assert.AreEqual(model.FirstName , "John");
			Assert.AreEqual(model.LastName , "Doe");
			Assert.AreEqual(model.Id , 1);
			Assert.AreEqual(model.PhoneNumber , "7145551212");
			Assert.AreEqual(model.EmailAddress , "noreply@noreply.com");
		}
	}
}
