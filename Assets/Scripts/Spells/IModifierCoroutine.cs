using System.Collections;  // <== THIS is required for IEnumerator
using UnityEngine;

public interface IModifierCoroutine
{
    IEnumerator CastWithCoroutine(Spell spell, Vector3 where, Vector3 target);
}