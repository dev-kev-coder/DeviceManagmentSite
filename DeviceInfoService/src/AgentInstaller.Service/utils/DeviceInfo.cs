using AgentInstaller.Service.utils.wmiClassOptions;
using AgentInstaller.Service.utils.wmiClassOptions.definitions;
using Microsoft.Management.Infrastructure;
using System.Reflection;

namespace AgentInstaller.Service.utils
{
    internal class DeviceInfo
    {
        public DeviceInfo() 
        {
        }
      

        public static T CreateAndPopulateV2<T>(Func< string, Type, object> getValue) where T : class
        {
            // Attempt to create instance of type T
            var obj = Activator.CreateInstance(typeof(T)) as T;

            // Guard against creation errors
            if (obj == null) throw new Exception($"Could not create instance of type: {typeof(T)}");

            // Get public properties of a type that inherits class
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanWrite);

            // Go through each property and process it
            foreach (var prop in props)
            {
                var value = getValue(prop.Name, prop.PropertyType);

                prop.SetValue(obj, value, null);
            }

            return obj;
        }


        public static void GetDeviceInfoWMI()
        {
            var wmiOptionQuerier = new WMIOptionQuerier();

            var option1 = wmiOptionQuerier
                .CreateQueryOption<Win32_BIOS>("Win32_BIOS")
                .CreateAndPopulate();

            var option2 = wmiOptionQuerier
                .CreateQueryOption<Win32_DiskDrive>("Win32_DiskDrive")
                .CreateAndPopulate();

            //var option3 = wmiOptionQuerier
            //    .CreateQueryOption<Win32_Directory>("Win32_Directory")
            //    .CreateAndPopulate();

            var stop = "here";

            //var option1 = new WMIQueryOption<Win32_BIOS>("Win32_BIOS");
            //var option2 = new WMIQueryOption<Win32_DiskDrive>("Win32_DiskDrive");

            //var option1 = new WMIQueryOptionV2<Win32_BIOS>("Win32_BIOS")
            //    .CreateAndPopulate();

            //var option2 = new WMIQueryOptionV2<Win32_DiskDrive>("Win32_DiskDrive")
            //    .CreateAndPopulate();


            //var wmiNamespace = @"root\cimv2";
            //var diskDriveQuery = "SELECT * FROM Win32_BIOS";
            //var wmiQuerier = new CimQuerier();
            //var results = wmiQuerier
            //    .QueryWMI(diskDriveQuery)
            //    .Select(res =>
            //    {
            //        var cimProps = res.CimInstanceProperties;

            //        //var constructionator = new MagicAutoConstructinator<Win32_BIOSV2>();

            //        //var test = constructionator.CreateAndPopulate((propName, propType) =>
            //        //{
            //        //    return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
            //        //});

            //        //return test;

            //        var win32BiosInfo = CreateAndPopulateV2<Win32_BIOS>((propName, propType) =>
            //        {
            //            //return TypeCaster.Cast(cimProps[propName].Value, propType);
            //            return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
            //        });

            //        return win32BiosInfo;
            //    }).ToArray();

        }
    }

    //public class CimQuerier
    //{
    //    private string _wmiNamespace = @"root\cimv2";

    //    private string _queryStructure = "WQL";

    //    private CimSession _defaultCimSession = null;

    //    public CimQuerier()
    //    {
    //        _defaultCimSession = CimSession.Create(null);
    //    }

    //    public CimQuerier(string wmiNameSpace, string queryStructure)
    //    {
    //        _wmiNamespace = wmiNameSpace;
    //        _queryStructure = queryStructure;
    //        _defaultCimSession = CimSession.Create(null);
    //    }

    //    public IEnumerable<CimInstance> QueryWMI(string query)
    //    {
    //        return _defaultCimSession.QueryInstances(_wmiNamespace, _queryStructure, query);
    //    }
    //}
}
