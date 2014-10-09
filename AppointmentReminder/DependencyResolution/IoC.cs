using StructureMap;
namespace AppointmentReminder {
	using AppointmentReminder.Data;

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
                        });
            return ObjectFactory.Container;
        }
    }
}