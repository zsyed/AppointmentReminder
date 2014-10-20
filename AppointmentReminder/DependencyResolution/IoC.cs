using StructureMap;
namespace AppointmentReminder {
	using System.Web.Http;
	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

	using WebApiContrib.IoC.StructureMap;

	public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<IReminderDb>().HttpContextScoped().Use<ReminderDb>();
							x.For<IProfileModel>().HttpContextScoped().Use<ProfileModel>();
							x.For<IIdentityService>().HttpContextScoped().Use<IdentityService>();
                        });
			GlobalConfiguration.Configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            return ObjectFactory.Container;
        }
    }
}