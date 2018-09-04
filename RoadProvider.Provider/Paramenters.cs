using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadProvider.Provider
{
    public class Parameters
    {
        private string _regionCodeParamName = ConfigurationManager.AppSettings["apiParameterNameRegionCode"];
        private string _bustCacheParamName = ConfigurationManager.AppSettings["apiParameterNameBustCache"];

        private readonly NameValueCollection _collection;

        public string Region
        {
            get { return _collection[_regionCodeParamName]; }
            set { _collection[_regionCodeParamName] = value; }
        }

        public string BustCache
        {
            get { return _collection[_bustCacheParamName]; }
            set { _collection[_bustCacheParamName] = value; }
        }


        public Parameters()
        {
            _collection = new NameValueCollection();
        }

        public NameValueCollection ParamsAsCollection()
        {
            return _collection;
        }
    }
}
