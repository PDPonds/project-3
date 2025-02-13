using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    public bool IsLocked { get; set; }
    public int MaxLockCount { get; set; }
    public int StartShowLockCount { get; set; }
    public List<int> LockPos { get; set; }
    public void ActionAfterUnlock();

}
