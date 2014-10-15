using StructureMap;
namespace AppointmentReminder {
	using AppointmentReminder.Data;
	using AppointmentReminder.Models;

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
                        });
            return ObjectFactory.Container;
        }
    }
}