using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AGS.Installers
{
    [CreateAssetMenu(fileName = "GraberInstaller", menuName = "Zenject/Installers/GraberInstaller")]
    public class GraberInstallerSettings : ScriptableObjectInstaller<GraberInstallerSettings>
    {
        public GraberInstaller.ShipSectionObjects ShipObjacts;
        public override void InstallBindings()
        {
            Container.BindInstance(ShipObjacts);
        }
    }
}
