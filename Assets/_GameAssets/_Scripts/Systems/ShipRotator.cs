using Zenject;

class ShipRotator : TransformRotator
{
    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        if (enabled)
        {
            enabled = false;
        }

        gameStateService.OnGameStarted += () => enabled = true;
    }
}
