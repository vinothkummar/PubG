using Fanview.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Assets
{
    public class DamageCauserName
    {
        private ICacheService _cacheService;
        public DamageCauserName(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Dictionary<string, string>> GetDamageCauserName()
        {
            var DamageCauserCache = _cacheService.RetrieveFromCache<Dictionary<string, string>>("DamageCauserCache");

            if (DamageCauserCache != null)
            {
                return DamageCauserCache;
            }
            else
            {
                Dictionary<string, string> damageCauser = new Dictionary<string, string>();

                damageCauser.Add("AquaRail_A_01_C", "Aquarail");
                damageCauser.Add("AquaRail_A_02_C", "Aquarail");
                damageCauser.Add("AquaRail_A_03_C", "Aquarail");
                damageCauser.Add("BattleRoyaleModeController_Def_C", "Bluezone");
                damageCauser.Add("BattleRoyaleModeController_Desert_C", "Bluezone");
                damageCauser.Add("BattleRoyaleModeController_Savage_C", "Bluezone");
                damageCauser.Add("Boat_PG117_C", "PG-117");
                damageCauser.Add("BP_Mirado_A_02_C", "Mirado");
                damageCauser.Add("BP_Mirado_Open_03_C", "Mirado (open top)");
                damageCauser.Add("BP_Mirado_Open_04_C", "Mirado (open top)");
                damageCauser.Add("BP_MolotovFireDebuff_C", "Molotov Fire Damage");
                damageCauser.Add("BP_Motorbike_04_C", "Motorcycle");
                damageCauser.Add("BP_Motorbike_04_Desert_C", "Motorcycle");
                damageCauser.Add("BP_Motorbike_04_SideCar_C", "Motorcycle (w/ Sidecar)");
                damageCauser.Add("BP_Motorbike_04_SideCar_Desert_C", "Motorcycle (w/ Sidecar)");
                damageCauser.Add("BP_PickupTruck_A_01_C", "Pickup Truck (closed top)");
                damageCauser.Add("BP_PickupTruck_A_02_C", "Pickup Truck (closed top)");
                damageCauser.Add("BP_PickupTruck_A_03_C", "Pickup Truck (closed top)");
                damageCauser.Add("BP_PickupTruck_A_04_C", "Pickup Truck (closed top)");
                damageCauser.Add("BP_PickupTruck_A_05_C", "Pickup Truck (closed top)");
                damageCauser.Add("BP_PickupTruck_B_01_C", "Pickup Truck (open top)");
                damageCauser.Add("BP_PickupTruck_B_02_C", "Pickup Truck (open top)");
                damageCauser.Add("BP_PickupTruck_B_03_C", "Pickup Truck (open top)");
                damageCauser.Add("BP_PickupTruck_B_04_C", "Pickup Truck (open top)");
                damageCauser.Add("BP_PickupTruck_B_05_C", "Pickup Truck (open top)");
                damageCauser.Add("BP_Van_A_01_C", "Van");
                damageCauser.Add("BP_Van_A_02_C", "Van");
                damageCauser.Add("BP_Van_A_03_C", "Van");
                damageCauser.Add("Buff_DecreaseBreathInApnea_C", "Drowning");
                damageCauser.Add("Buggy_A_01_C", "Buggy");
                damageCauser.Add("Buggy_A_02_C", "Buggy");
                damageCauser.Add("Buggy_A_03_C", "Buggy");
                damageCauser.Add("Buggy_A_04_C", "Buggy");
                damageCauser.Add("Buggy_A_05_C", "Buggy");
                damageCauser.Add("Buggy_A_06_C", "Buggy");
                damageCauser.Add("Dacia_A_01_v2_C", "Dacia");
                damageCauser.Add("Dacia_A_02_v2_C", "Dacia");
                damageCauser.Add("Dacia_A_03_v2_C", "Dacia");
                damageCauser.Add("Dacia_A_04_v2_C", "Dacia");
                damageCauser.Add("None", "None");
                damageCauser.Add("PG117_A_01_C", "PG-117");
                damageCauser.Add("PlayerFemale_A_C", "Player");
                damageCauser.Add("PlayerMale_A_C", "Player");
                damageCauser.Add("ProjGrenade_C", "Grenade");
                damageCauser.Add("ProjMolotov_C", "Molotov Cocktail");
                damageCauser.Add("ProjMolotov_DamageField_Direct_C", "Molotov Cocktail Fire Field");
                damageCauser.Add("RedZoneBomb_C", "Redzone");
                damageCauser.Add("Uaz_A_01_C", "UAZ (open top)");
                damageCauser.Add("Uaz_B_01_C", "UAZ (soft top)");
                damageCauser.Add("Uaz_C_01_C", "UAZ (hard top)");
                damageCauser.Add("WeapAK47_C", "AKM");
                damageCauser.Add("WeapAUG_C", "AUG A3");
                damageCauser.Add("WeapAWM_C", "AWM");
                damageCauser.Add("WeapBerreta686_C", "S686");
                damageCauser.Add("WeapCowbar_C", "Crowbar");
                damageCauser.Add("WeapCrossbow_1_C", "Crossbow");
                damageCauser.Add("WeapDP28_C", "DP-28");
                damageCauser.Add("WeapFNFal_C", "SLR");
                damageCauser.Add("WeapG18_C", "P18C");
                damageCauser.Add("WeapGroza_C", "Groza");
                damageCauser.Add("WeapHK416_C", "M416");
                damageCauser.Add("WeapKar98k_C", "Kar98k");
                damageCauser.Add("WeapM16A4_C", "M16A4");
                damageCauser.Add("WeapM1911_C", "P1911");
                damageCauser.Add("WeapM249_C", "M249");
                damageCauser.Add("WeapM24_C", "M24");
                damageCauser.Add("WeapM9_C", "P92");
                damageCauser.Add("WeapMachete_C", "Machete");
                damageCauser.Add("WeapMini14_C", "Mini 14");
                damageCauser.Add("WeapMk14_C", "Mk14 EBR");
                damageCauser.Add("WeapNagantM1895_C", "R1895");
                damageCauser.Add("WeapPan_C", "Pan");
                damageCauser.Add("WeapQBZ95_C", "QBZ95");
                damageCauser.Add("WeapRhino_C", "R45");
                damageCauser.Add("WeapSaiga12_C", "S12K");
                damageCauser.Add("WeapSawnoff_C", "Sawed-off");
                damageCauser.Add("WeapSCAR-L_C", "SCAR-L");
                damageCauser.Add("WeapSickle_C", "Sickle");
                damageCauser.Add("WeapSKS_C", "SKS");
                damageCauser.Add("WeapThompson_C", "Tommy Gun");
                damageCauser.Add("WeapUMP_C", "UMP9");
                damageCauser.Add("WeapUZI_C", "Micro Uzi");
                damageCauser.Add("WeapVector_C", "Vector");
                damageCauser.Add("WeapVSS_C", "VSS");
                damageCauser.Add("WeapWin94_C", "Win94");
                damageCauser.Add("WeapWinchester_C", "S1897");

                await _cacheService.SaveToCache<Dictionary<string, string>>("DamageCauserCache", damageCauser, 45, 7);

                return damageCauser;
            }
        }
    }
}
