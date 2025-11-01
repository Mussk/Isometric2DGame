using UnityEngine;

public interface IHasHealth 
{
    public Health HealthSystem { get; set; }
    
    public HealthBar HealthBar { get; set; }
}
