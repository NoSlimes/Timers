# Timers
Simple timers class for Unity

## Setup
You just need to add the TimerManager component to a GameObject in your scene - thats it!

## Creating a Basic Timer
You can create a timer using the TimerManager and specify a callback function to execute when the timer finishes:

```cs
using NSJC.Timers;

public class MyClass
{
    bool hasCooldown = false;

    private void UseAbility(Ability ability)
    {
        ability.Use();
        hasCooldown = true;
        Timer timer = TimerManager.Instance.CreateTimer(1f, TimerFinished);
        timer.Play(); // OR TimerManager.Instance.PlayTimer(timer);
    }

    private void TimerFinished()
    {
        hasCooldown = false;
    }
}
```

## Assigning a Reference to a Timer
You can create a timer in the Start method and assign it to a field for later use:

```cs
using NSJC.Timers;

public class MyClass : MonoBehaviour
{
    bool hasCooldown = false;
    private Timer timer;

    private void Start()
    {
        timer = TimerManager.Instance.CreateTimer(1f, TimerFinished);
    }

    private void UseAbility(Ability ability)
    {
        ability.Use();
        timer.Play(); // OR TimerManager.Instance.PlayTimer(timer);
    }

    private void TimerFinished()
    {
        hasCooldown = false;
    }
}
```

## Using a Lambda Expression for OnTimerFinished
You can use a lambda expression directly in the CreateTimer method to define what happens when the timer finishes:
```cs
using NSJC.Timers;

public class MyClass
{
    bool hasCooldown = false;

    private void UseAbility(Ability ability)
    {
        ability.Use();
        hasCooldown = true;
        TimerManager.Instance.CreateTimer(1f, () => hasCooldown = false, startOnCreation: true, destroyOnCompletion: true);
    }
}
```

## Additional Timer Operations
You can start, reset, and change the duration of a timer as needed:
```cs
using NSJC.Timers;

class MyClass : MonoBehaviour
{
    Timer timer;

    private void Start()
    {
        timer = TimerManager.Instance.CreateTimer(50f, TimerFinished);
    }

    private void StartTimer()
    {
        timer.Play();
    }

    private void ResetTimer()
    {
        timer.Reset(pause: true);
    }

    private void SetTimerDuration()
    {
        timer.SetDuration(23f, pause: true);
    }

    private void TimerFinished()
    {
        Debug.Log("TIMER FINISHED!");
    }
}
```
