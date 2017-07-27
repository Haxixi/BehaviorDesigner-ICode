﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICode.FSMEditor{
	[InitializeOnLoad]
	public class EnableICodeScripting {
		private const string name = "ICODE";
		
		static EnableICodeScripting (){
			List<BuildTargetGroup> buildTargets = Enum.GetValues(typeof(BuildTargetGroup)).Cast<BuildTargetGroup>().Where(x=>!IsObsolete(x)).ToList();
			buildTargets.Remove (BuildTargetGroup.Unknown);
			
			foreach (BuildTargetGroup group in buildTargets)
			{
				if(!IsEnabled(group,name)){
					string symbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
					string[] split = symbols.Split(';');
					var list = new List<string>(split);
					list.Add(name);
					UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", list.ToArray()));
				}
			}
		}
		
		private static bool IsEnabled(BuildTargetGroup targetGroup, string name)
		{
			return UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Contains(name);
		}
		
		private static bool IsObsolete(Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (ObsoleteAttribute[])
				fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
			
			return (attributes != null && attributes.Length > 0);
		}
	}
}