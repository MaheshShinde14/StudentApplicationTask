using StudentApp.BAL;
using StudentApp.DAL;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace StudentApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IClassDataAccessLayer, ClassDataAccessLayer>();
            container.RegisterType<IClassServiceInterface, ClassService>();
            container.RegisterType<IStudentDataAccessLayer, StudentDataAccessLayer>();
            container.RegisterType<IStudentServiceInterface, StudentService>();
            RegisterMvcComponents(container);
            RegisterWebApiComponents(container);
            
            container.RegisterType<IClassServiceInterface, ClassService>(new HierarchicalLifetimeManager());
            container.RegisterType<IStudentServiceInterface, StudentService>(new HierarchicalLifetimeManager());

            container.RegisterType<IClassDataAccessLayer, ClassDataAccessLayer>(new HierarchicalLifetimeManager());
            container.RegisterType<IStudentDataAccessLayer, StudentDataAccessLayer>(new HierarchicalLifetimeManager());

        }
        private static void RegisterMvcComponents(IUnityContainer container)
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterWebApiComponents(IUnityContainer container)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}