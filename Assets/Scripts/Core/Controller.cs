using UnityEngine;

public interface Controller
{
    Character character { get; set; }  
    HealthBar HealthUI { get; set; }
    bool IsDead { get; set; }
    void Die(); 
}