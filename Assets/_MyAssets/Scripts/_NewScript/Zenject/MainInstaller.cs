using GameManagement;
using GameRules;
using Players;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] Player player;
    [SerializeField] TimeController timeController;
    public override void InstallBindings()
    {
        //gameRule‚Ìbind
        Container.Bind<IPlayer>().To<Player>().FromComponentOn(player.gameObject).AsCached();
        Container.Bind<ITimeController>().To<TimeController>().FromComponentOn(timeController.gameObject).AsCached();
    }
}