using System;
using UnityEngine;

namespace ES3Types
{
	[ES3PropertiesAttribute("Date", "Score", "HighestLevel")]
	public class ES3Type_ScoreItem : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3Type_ScoreItem() : base(typeof(Assets.Classes.ScoreItem)){ Instance = this; }

		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Classes.ScoreItem)obj;
			
			writer.WriteProperty("Date", instance.Date, ES3Type_DateTime.Instance);
			writer.WriteProperty("Score", instance.Score, ES3Type_int.Instance);
			writer.WriteProperty("HighestLevel", instance.HighestLevel, ES3Type_int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Classes.ScoreItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Date":
						instance.Date = reader.Read<System.DateTime>(ES3Type_DateTime.Instance);
						break;
					case "Score":
						instance.Score = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "HighestLevel":
						instance.HighestLevel = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Classes.ScoreItem();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}

	public class ES3Type_ScoreItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_ScoreItemArray() : base(typeof(Assets.Classes.ScoreItem[]), ES3Type_ScoreItem.Instance)
		{
			Instance = this;
		}
	}
}