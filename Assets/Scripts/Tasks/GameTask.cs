using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    protected string description;

    public abstract bool isComplete();

    protected abstract void updateProgress();
}
