using System;
using System.Collections.Generic;
using UnityEngine;
using static TheOtherRoles.Helper.LogHelper;
using BepInEx.Configuration;
using HarmonyLib;

namespace TheOtherRoles.Modules;

internal class LateTask
{
    public string name;
    public float timer;
    public Action action;
    public static List<LateTask> Tasks = new();
    public bool Run(float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0)
        {
            action();
            return true;
        }
        return false;
    }

    public LateTask(Action action, float time, string name = "No Name Task")
    {
        this.action = action;
        timer = time;
        this.name = name;
        Tasks.Add(this);

        Info("New LateTask \"" + name + "\" is created");
    }

    public static void Update(float deltaTime)
    {
        var TasksToRemove = new List<LateTask>();
        for (int i = 0; i < Tasks.Count; i++)
        {
            var task = Tasks[i];
            try
            {
                if (task.Run(deltaTime))
                {
                    TasksToRemove.Add(task);
                }
            }
            catch (Exception)
            {
                TasksToRemove.Add(task);
            }
        }
        TasksToRemove.ForEach(task => Tasks.Remove(task));
    }
}

/*[HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
internal class ModManagerLateUpdatePatch
{
    public static void Prefix(ModManager __instance)
    {
        __instance.ShowModStamp();
        LateTask.Update(Time.fixedDeltaTime / 2);

    }
}*/