using UnityEngine.Events;
using System.Collections.Generic;

public enum UIEvents
{
    None
}


public class EventController : Singleton<EventController>
{

    private Dictionary<UIEvents, UnityEvent> EventDatabase;
    private UnityEvent eventBuffer;
    // Use this for initialization
    void Awake()
    {
        this.InitEventDatabase();
    }


    #region Init Functions
    void InitEventDatabase()
    {
        if (this.EventDatabase == null)
        {
            this.EventDatabase = new Dictionary<UIEvents, UnityEvent>();
        }
    }
    #endregion
    #region Event Subscription
    public void SubscribeEvent(UIEvents eventDesignation, UnityAction listener)
    {
        this.eventBuffer = null;
        if (this.EventDatabase.TryGetValue(eventDesignation, out eventBuffer))
        {
            this.eventBuffer.AddListener(listener);
        }
        else
        {
            this.eventBuffer = new UnityEvent();
            this.eventBuffer.AddListener(listener);
            this.EventDatabase.Add(eventDesignation, this.eventBuffer);
        }
    }

    public void UnSubscribeEvent(UIEvents eventDesignation, UnityAction listener)
    {
        if (!Instance)
        {
            return;
        }

        this.eventBuffer = null;
        if (this.EventDatabase.TryGetValue(eventDesignation, out this.eventBuffer))
        {
            this.eventBuffer.RemoveListener(listener);
        }


    }

    void UnSubscribeAllEvents()
    {
        foreach (UIEvents uiEvt in this.EventDatabase.Keys)
        {
            EventDatabase[uiEvt].RemoveAllListeners();
        }
    }

    public void TriggerEvent(UIEvents eventDesignation)
    {
        this.eventBuffer = null;

        if (this.EventDatabase.TryGetValue(eventDesignation, out this.eventBuffer))
        {
            this.eventBuffer.Invoke();
        }
    }
    #endregion
    void OnDisable()
    {
        this.UnSubscribeAllEvents();
    }
}

