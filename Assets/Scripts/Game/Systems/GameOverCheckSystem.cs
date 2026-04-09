using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(GameStateSystemGroup))]
public partial struct GameOverCheckSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameProcessState>();
        state.RequireForUpdate<PlayerData>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        bool anyAlive = false;
        
        foreach (var (playerData, _) in SystemAPI.Query<RefRO<PlayerData>, RefRO<PlayerIndex>>()) 
            anyAlive |= playerData.ValueRO.Lives != 0;

        if (!anyAlive)
            state.EntityManager.AddSingleFrameComponent(ChangeStateCommand.Create<GameOverState>());
    }
}
