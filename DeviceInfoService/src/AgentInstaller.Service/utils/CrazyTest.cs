//using System.Reflection;

//namespace AgentInstaller.Service.utils
//{
//    //public class WMIQueryOption<T>
//    //{
//    //    public string WMIClassName { get; set; }
//    //    public WMIQueryOption(string className) 
//    //    {
//    //        WMIClassName = className;
//    //    }

//    //    public T CreateAndPopulate(Func<string, Type, object> getValue) 
//    //    {
//    //        var obj = this.CreateInstance();

//    //        // Guard against creation errors
//    //        if (obj == null) throw new Exception($"Could not create instance of type: {typeof(T)}");

//    //        // Get public properties of a type that inherits class
//    //        var props = typeof(T)
//    //            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
//    //            .Where(prop => prop.CanWrite);

//    //        // Go through each property and process it
//    //        foreach (var prop in props)
//    //        {
//    //            var value = getValue(prop.Name, prop.PropertyType);

//    //            prop.SetValue(obj, value, null);
//    //        }

//    //        return obj;
//    //    }

//    //    private T CreateInstance()
//    //    {
//    //        return Activator.CreateInstance<T>();
//    //    }
//    //}

//    public class WMIQueryOptionV2<T>
//    {
//        private CimQuerier _querier;
//        public string WMIClassName { get; set; }
        
//        public WMIQueryOptionV2(string className, CimQuerier? querier = null)
//        {
//            WMIClassName = className;

//            // If no querier is provided then we will make one
//            if (querier != null) 
//            {
//                _querier = querier;
//            }
//            else
//            {
//                _querier = new CimQuerier();
//            }
//        }


//        public T[] CreateAndPopulate()
//        {
//            var queryResults = _querier
//                .QueryWMI($"SELECT * FROM {this.WMIClassName}")
//                .Select(res =>
//                {
//                    var cimProps = res.CimInstanceProperties;

//                    var constructedOjb = this.CreateAndPopulate((propName, propType) =>
//                    {
//                        return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
//                    });

//                    return constructedOjb;
//                });

//            return queryResults.ToArray();
//        }

//        public T CreateAndPopulate(Func<string, Type, object> getValue)
//        {
//            var obj = this.CreateInstance();

//            // Guard against creation errors
//            if (obj == null) throw new Exception($"Could not create instance of type: {typeof(T)}");

//            // Get public properties of a type that inherits class
//            var props = typeof(T)
//                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                .Where(prop => prop.CanWrite);

//            // Go through each property and process it
//            foreach (var prop in props)
//            {
//                var value = getValue(prop.Name, prop.PropertyType);

//                prop.SetValue(obj, value, null);
//            }

//            return obj;
//        }

//        private T CreateInstance()
//        {
//            return Activator.CreateInstance<T>();
//        }
//    }
//}
