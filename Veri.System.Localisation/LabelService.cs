using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using Veri.System.Data;
using Veri.System.Services;

namespace Veri.System.Localisation
{
    public class LabelService : ILabelService
    {
        private readonly IRedisTypedClient<Label> db;

        public LabelService(ICacheContext cache)
        {
            db = cache.GetDb<Label>();
        }

        public string Get(string code, string iso = "en")
        {
            var ret = code;

            var label = db.GetById(code);
            if (label != null
                && label.Labels != null
                && label.Labels.Any(i => i.Iso == iso))
            {
                var labels = label.Labels
                    .Where(i => i.Iso == iso)
                    .Select(i => i.Label);

                if(labels.Any())
                {
                    ret = labels.First();
                }
            }

            return ret;
        }

        public string Get(string code, string iso, params string[] inputs)
        {
            var ret = Get(code, iso);
            return string.Format(ret, inputs);
        }
    }
}
