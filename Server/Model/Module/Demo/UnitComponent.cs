﻿using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ETModel
{
    [ObjectSystem]
    public class UnitComponentAwakeSystem : AwakeSystem<UnitComponent>
    {
        public override void Awake(UnitComponent self)
        {
            UnitComponent.Instance = self;
        }
    }

    public class UnitComponent: Component
	{
		[BsonElement]
		[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
		private readonly Dictionary<long, Unit> idUnits = new Dictionary<long, Unit>();

        [BsonIgnore]
        public static UnitComponent Instance;

        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();

			foreach (Unit unit in this.idUnits.Values)
			{
				unit.Dispose();
			}
			this.idUnits.Clear();
		}

		public void Add(Unit unit)
		{
			this.idUnits.Add(unit.Id, unit);
		}

		public Unit Get(long id)
		{
			this.idUnits.TryGetValue(id, out Unit unit);
			return unit;
		}

		public void Remove(long id)
		{
			Unit unit;
			this.idUnits.TryGetValue(id, out unit);
			this.idUnits.Remove(id);
			unit?.Dispose();
		}

		public void RemoveNoDispose(long id)
		{
			this.idUnits.Remove(id);
		}

		public int Count
		{
			get
			{
				return this.idUnits.Count;
			}
		}

		public Unit[] GetAll()
		{
			return this.idUnits.Values.ToArray();
		}
	}
}