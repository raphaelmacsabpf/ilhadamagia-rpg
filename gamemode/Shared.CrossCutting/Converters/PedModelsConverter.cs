﻿using System.Collections.Generic;
using System.Linq;

namespace Shared.CrossCutting
{
    public class PedModelsConverter
    {
        static PedModelsConverter()
        {
            AllPedsDictionary = new Dictionary<string, string>()
            {
                // ["a_c_boar"] = "Boar", // ERROR
                ["a_c_cat_01"] = "Cat",
                // ["a_c_chickenhawk"] = "ChickenHawk", // ERROR
                // ["a_c_chimp"] = "Chimp", // ERROR
                ["a_c_chop"] = "Chop",
                ["a_c_cormorant"] = "Cormorant",
                // ["a_c_cow"] = "Cow", // ERROR
                ["a_c_coyote"] = "Coyote",
                ["a_c_crow"] = "Crow",
                // ["a_c_deer"] = "Deer", // ERROR
                // ["a_c_dolphin"] = "Dolphin", // ERROR
                ["a_c_fish"] = "Fish",
                ["a_c_hen"] = "Hen",
                ["a_c_humpback"] = "Humpback",
                ["a_c_husky"] = "Husky",
                // ["a_c_killerwhale"] = "KillerWhale", // ERROR
                ["a_c_mtlion"] = "MountainLion",
                // ["a_c_pig"] = "Pig", // ERROR
                ["a_c_pigeon"] = "Pigeon",
                ["a_c_poodle"] = "Poodle",
                ["a_c_pug"] = "Pug",
                // ["a_c_rabbit_01"] = "Rabbit", // ERROR
                // ["a_c_rat"] = "Rat", // ERROR
                ["a_c_retriever"] = "Retriever",
                // ["a_c_rhesus"] = "Rhesus", // ERROR
                // ["a_c_rottweiler"] = "Rottweiler", // ERROR
                // ["a_c_seagull"] = "Seagull", // ERROR
                ["a_c_sharkhammer"] = "HammerShark",
                ["a_c_sharktiger"] = "TigerShark",
                ["a_c_shepherd"] = "Shepherd",
                // ["a_c_westy"] = "Westy", // ERROR
                ["a_m_m_acult_01"] = "Acult01AMM",
                // ["a_m_m_afriamer_01"] = "AfriAmer01AMM", // ERROR
                ["a_m_m_beach_01"] = "Beach01AMM",
                ["a_m_m_beach_02"] = "Beach02AMM",
                ["a_m_m_bevhills_01"] = "Bevhills01AMM",
                ["a_m_m_bevhills_02"] = "Bevhills02AMM",
                ["a_m_m_business_01"] = "Business01AMM",
                // ["a_m_m_eastsa_01"] = "Eastsa01AMM", // ERROR
                ["a_m_m_eastsa_02"] = "Eastsa02AMM",
                // ["a_m_m_farmer_01"] = "Farmer01AMM", // ERROR
                ["a_m_m_fatlatin_01"] = "Fatlatin01AMM",
                ["a_m_m_genfat_01"] = "Genfat01AMM",
                ["a_m_m_genfat_02"] = "Genfat02AMM",
                // ["a_m_m_golfer_01"] = "Golfer01AMM", // ERROR
                ["a_m_m_hasjew_01"] = "Hasjew01AMM",
                ["a_m_m_hillbilly_01"] = "Hillbilly01AMM",
                ["a_m_m_hillbilly_02"] = "Hillbilly02AMM",
                // ["a_m_m_indian_01"] = "Indian01AMM", // ERROR
                // ["a_m_m_ktown_01"] = "Ktown01AMM", // ERROR
                ["a_m_m_malibu_01"] = "Malibu01AMM",
                // ["a_m_m_mexcntry_01"] = "MexCntry01AMM", // ERROR
                // ["a_m_m_mexlabor_01"] = "MexLabor01AMM", // ERROR
                ["a_m_m_og_boss_01"] = "OgBoss01AMM",
                // ["a_m_m_paparazzi_01"] = "Paparazzi01AMM", // ERROR
                // ["a_m_m_polynesian_01"] = "Polynesian01AMM", // ERROR
                // ["a_m_m_prolhost_01"] = "PrologueHostage01AMM", // ERROR
                ["a_m_m_rurmeth_01"] = "Rurmeth01AMM",
                ["a_m_m_salton_01"] = "Salton01AMM",
                ["a_m_m_salton_02"] = "Salton02AMM",
                // ["a_m_m_salton_03"] = "Salton03AMM", // ERROR
                // ["a_m_m_salton_04"] = "Salton04AMM", // ERROR
                // ["a_m_m_skater_01"] = "Skater01AMM", // ERROR
                ["a_m_m_skidrow_01"] = "Skidrow01AMM",
                ["a_m_m_socenlat_01"] = "Socenlat01AMM",
                ["a_m_m_soucent_01"] = "Soucent01AMM",
                // ["a_m_m_soucent_02"] = "Soucent02AMM", // ERROR
                // ["a_m_m_soucent_03"] = "Soucent03AMM", // ERROR
                // ["a_m_m_soucent_04"] = "Soucent04AMM", // ERROR
                // ["a_m_m_stlat_02"] = "Stlat02AMM", // ERROR
                ["a_m_m_tennis_01"] = "Tennis01AMM",
                // ["a_m_m_tourist_01"] = "Tourist01AMM", // ERROR
                ["a_m_m_trampbeac_01"] = "TrampBeac01AMM",
                ["a_m_m_tramp_01"] = "Tramp01AMM",
                // ["a_m_m_tranvest_01"] = "Tranvest01AMM", // ERROR
                // ["a_m_m_tranvest_02"] = "Tranvest02AMM", // ERROR
                ["a_m_o_acult_01"] = "Acult01AMO",
                ["a_m_o_acult_02"] = "Acult02AMO",
                // ["a_m_o_beach_01"] = "Beach01AMO", // ERROR
                // ["a_m_o_genstreet_01"] = "Genstreet01AMO", // ERROR
                ["a_m_o_ktown_01"] = "Ktown01AMO",
                ["a_m_o_salton_01"] = "Salton01AMO",
                ["a_m_o_soucent_01"] = "Soucent01AMO",
                ["a_m_o_soucent_02"] = "Soucent02AMO",
                ["a_m_o_soucent_03"] = "Soucent03AMO",
                ["a_m_o_tramp_01"] = "Tramp01AMO",
                // ["a_m_y_acult_01"] = "Acult01AMY", // ERROR
                // ["a_m_y_acult_02"] = "Acult02AMY", // ERROR
                ["a_m_y_beachvesp_01"] = "Beachvesp01AMY",
                // ["a_m_y_beachvesp_02"] = "Beachvesp02AMY", // ERROR
                // ["a_m_y_beach_01"] = "Beach01AMY", // ERROR
                ["a_m_y_beach_02"] = "Beach02AMY",
                // ["a_m_y_beach_03"] = "Beach03AMY", // ERROR
                ["a_m_y_bevhills_01"] = "Bevhills01AMY",
                ["a_m_y_bevhills_02"] = "Bevhills02AMY",
                ["a_m_y_breakdance_01"] = "Breakdance01AMY",
                // ["a_m_y_busicas_01"] = "Busicas01AMY", // ERROR
                // ["a_m_y_business_01"] = "Business01AMY", // ERROR
                // ["a_m_y_business_02"] = "Business02AMY", // ERROR
                // ["a_m_y_business_03"] = "Business03AMY", // ERROR
                // ["a_m_y_cyclist_01"] = "Cyclist01AMY", // ERROR
                // ["a_m_y_dhill_01"] = "Dhill01AMY", // ERROR
                ["a_m_y_downtown_01"] = "Downtown01AMY",
                // ["a_m_y_eastsa_01"] = "Eastsa01AMY", // ERROR
                ["a_m_y_eastsa_02"] = "Eastsa02AMY",
                ["a_m_y_epsilon_01"] = "Epsilon01AMY",
                // ["a_m_y_epsilon_02"] = "Epsilon02AMY", // ERROR
                // ["a_m_y_gay_01"] = "Gay01AMY", // ERROR
                // ["a_m_y_gay_02"] = "Gay02AMY", // ERROR
                // ["a_m_y_genstreet_01"] = "Genstreet01AMY", // ERROR
                ["a_m_y_genstreet_02"] = "Genstreet02AMY",
                // ["a_m_y_golfer_01"] = "Golfer01AMY", // ERROR
                // ["a_m_y_hasjew_01"] = "Hasjew01AMY", // ERROR
                ["a_m_y_hiker_01"] = "Hiker01AMY",
                ["a_m_y_hippy_01"] = "Hippy01AMY",
                ["a_m_y_hipster_01"] = "Hipster01AMY",
                ["a_m_y_hipster_02"] = "Hipster02AMY",
                ["a_m_y_hipster_03"] = "Hipster03AMY",
                ["a_m_y_indian_01"] = "Indian01AMY",
                ["a_m_y_jetski_01"] = "Jetski01AMY",
                // ["a_m_y_juggalo_01"] = "Juggalo01AMY", // ERROR
                ["a_m_y_ktown_01"] = "Ktown01AMY",
                ["a_m_y_ktown_02"] = "Ktown02AMY",
                ["a_m_y_latino_01"] = "Latino01AMY",
                ["a_m_y_methhead_01"] = "Methhead01AMY",
                ["a_m_y_mexthug_01"] = "MexThug01AMY",
                ["a_m_y_motox_01"] = "Motox01AMY",
                ["a_m_y_motox_02"] = "Motox02AMY",
                ["a_m_y_musclbeac_01"] = "Musclbeac01AMY",
                // ["a_m_y_musclbeac_02"] = "Musclbeac02AMY", // ERROR
                // ["a_m_y_polynesian_01"] = "Polynesian01AMY", // ERROR
                // ["a_m_y_roadcyc_01"] = "Roadcyc01AMY", // ERROR
                ["a_m_y_runner_01"] = "Runner01AMY",
                // ["a_m_y_runner_02"] = "Runner02AMY", // ERROR
                // ["a_m_y_salton_01"] = "Salton01AMY", // ERROR
                // ["a_m_y_skater_01"] = "Skater01AMY", // ERROR
                // ["a_m_y_skater_02"] = "Skater02AMY", // ERROR
                // ["a_m_y_soucent_01"] = "Soucent01AMY", // ERROR
                // ["a_m_y_soucent_02"] = "Soucent02AMY", // ERROR
                // ["a_m_y_soucent_03"] = "Soucent03AMY", // ERROR
                // ["a_m_y_soucent_04"] = "Soucent04AMY", // ERROR
                // ["a_m_y_stbla_01"] = "Stbla01AMY", // ERROR
                // ["a_m_y_stbla_02"] = "Stbla02AMY", // ERROR
                // ["a_m_y_stlat_01"] = "Stlat01AMY", // ERROR
                ["a_m_y_stwhi_01"] = "Stwhi01AMY",
                ["a_m_y_stwhi_02"] = "Stwhi02AMY",
                // ["a_m_y_sunbathe_01"] = "Sunbathe01AMY", // ERROR
                // ["a_m_y_surfer_01"] = "Surfer01AMY", // ERROR
                // ["a_m_y_vindouche_01"] = "Vindouche01AMY", // ERROR
                ["a_m_y_vinewood_01"] = "Vinewood01AMY",
                ["a_m_y_vinewood_02"] = "Vinewood02AMY",
                ["a_m_y_vinewood_03"] = "Vinewood03AMY",
                ["a_m_y_vinewood_04"] = "Vinewood04AMY",
                // ["a_m_y_yoga_01"] = "Yoga01AMY", // ERROR
                ["a_f_m_beach_01"] = "Beach01AFM",
                // ["a_f_m_bevhills_01"] = "Bevhills01AFM", // ERROR
                // ["a_f_m_bevhills_02"] = "Bevhills02AFM", // ERROR
                ["a_f_m_bodybuild_01"] = "Bodybuild01AFM",
                ["a_f_m_business_02"] = "Business02AFM",
                ["a_f_m_downtown_01"] = "Downtown01AFM",
                // ["a_f_m_eastsa_01"] = "Eastsa01AFM", // ERROR
                ["a_f_m_eastsa_02"] = "Eastsa02AFM",
                // ["a_f_m_fatbla_01"] = "FatBla01AFM", // ERROR
                // ["a_f_m_fatcult_01"] = "FatCult01AFM", // ERROR
                ["a_f_m_fatwhite_01"] = "FatWhite01AFM",
                ["a_f_m_ktown_01"] = "Ktown01AFM",
                ["a_f_m_ktown_02"] = "Ktown02AFM",
                ["a_f_m_prolhost_01"] = "PrologueHostage01AFM",
                // ["a_f_m_salton_01"] = "Salton01AFM", // ERROR
                // ["a_f_m_skidrow_01"] = "Skidrow01AFM", // ERROR
                // ["a_f_m_soucentmc_01"] = "Soucentmc01AFM", // ERROR
                ["a_f_m_soucent_01"] = "Soucent01AFM",
                // ["a_f_m_soucent_02"] = "Soucent02AFM", // ERROR
                ["a_f_m_tourist_01"] = "Tourist01AFM",
                // ["a_f_m_trampbeac_01"] = "TrampBeac01AFM", // ERROR
                ["a_f_m_tramp_01"] = "Tramp01AFM",
                ["a_f_o_genstreet_01"] = "Genstreet01AFO",
                // ["a_f_o_indian_01"] = "Indian01AFO", // ERROR
                ["a_f_o_ktown_01"] = "Ktown01AFO",
                // ["a_f_o_salton_01"] = "Salton01AFO", // ERROR
                ["a_f_o_soucent_01"] = "Soucent01AFO",
                // ["a_f_o_soucent_02"] = "Soucent02AFO", // ERROR
                // ["a_f_y_beach_01"] = "Beach01AFY", // ERROR
                ["a_f_y_bevhills_01"] = "Bevhills01AFY",
                ["a_f_y_bevhills_02"] = "Bevhills02AFY",
                ["a_f_y_bevhills_03"] = "Bevhills03AFY",
                ["a_f_y_bevhills_04"] = "Bevhills04AFY",
                ["a_f_y_business_01"] = "Business01AFY",
                ["a_f_y_business_02"] = "Business02AFY",
                // ["a_f_y_business_03"] = "Business03AFY", // ERROR
                // ["a_f_y_business_04"] = "Business04AFY", // ERROR
                // ["a_f_y_eastsa_01"] = "Eastsa01AFY", // ERROR
                ["a_f_y_eastsa_02"] = "Eastsa02AFY",
                ["a_f_y_eastsa_03"] = "Eastsa03AFY",
                ["a_f_y_epsilon_01"] = "Epsilon01AFY",
                ["a_f_y_fitness_01"] = "Fitness01AFY",
                ["a_f_y_fitness_02"] = "Fitness02AFY",
                ["a_f_y_genhot_01"] = "Genhot01AFY",
                ["a_f_y_golfer_01"] = "Golfer01AFY",
                ["a_f_y_hiker_01"] = "Hiker01AFY",
                ["a_f_y_hippie_01"] = "Hippie01AFY",
                // ["a_f_y_hipster_01"] = "Hipster01AFY", // ERROR
                // ["a_f_y_hipster_02"] = "Hipster02AFY", // ERROR
                // ["a_f_y_hipster_03"] = "Hipster03AFY", // ERROR
                ["a_f_y_hipster_04"] = "Hipster04AFY",
                ["a_f_y_indian_01"] = "Indian01AFY",
                // ["a_f_y_juggalo_01"] = "Juggalo01AFY", // ERROR
                // ["a_f_y_runner_01"] = "Runner01AFY", // ERROR
                ["a_f_y_rurmeth_01"] = "Rurmeth01AFY",
                // ["a_f_y_scdressy_01"] = "Scdressy01AFY", // ERROR
                ["a_f_y_skater_01"] = "Skater01AFY",
                ["a_f_y_soucent_01"] = "Soucent01AFY",
                ["a_f_y_soucent_02"] = "Soucent02AFY",
                // ["a_f_y_soucent_03"] = "Soucent03AFY", // ERROR
                ["a_f_y_tennis_01"] = "Tennis01AFY",
                // ["a_f_y_topless_01"] = "Topless01AFY", // ERROR
                ["a_f_y_tourist_01"] = "Tourist01AFY",
                // ["a_f_y_tourist_02"] = "Tourist02AFY", // ERROR
                ["a_f_y_vinewood_01"] = "Vinewood01AFY",
                // ["a_f_y_vinewood_02"] = "Vinewood02AFY", // ERROR
                ["a_f_y_vinewood_03"] = "Vinewood03AFY",
                // ["a_f_y_vinewood_04"] = "Vinewood04AFY", // ERROR
                // ["a_f_y_yoga_01"] = "Yoga01AFY", // ERROR
                // ["csb_abigail"] = "AbigailCutscene", // ERROR
                ["csb_anita"] = "AnitaCutscene",
                // ["csb_anton"] = "AntonCutscene", // ERROR
                // ["csb_ballasog"] = "BallasogCutscene", // ERROR
                // ["csb_bride"] = "BrideCutscene", // ERROR
                // ["csb_burgerdrug"] = "BurgerDrugCutscene", // ERROR
                ["csb_car3guy1"] = "Car3Guy1Cutscene",
                ["csb_car3guy2"] = "Car3Guy2Cutscene",
                // ["csb_chef"] = "ChefCutscene", // ERROR
                // ["csb_chin_goon"] = "ChinGoonCutscene", // ERROR
                // ["csb_cletus"] = "CletusCutscene", // ERROR
                // ["csb_cop"] = "CopCutscene", // ERROR
                // ["csb_customer"] = "CustomerCutscene", // ERROR
                // ["csb_denise_friend"] = "DeniseFriendCutscene", // ERROR
                ["csb_fos_rep"] = "FosRepCutscene",
                ["csb_groom"] = "GroomCutscene",
                // ["csb_grove_str_dlr"] = "GroveStrDlrCutscene", // ERROR
                // ["csb_g"] = "GCutscene", // ERROR
                // ["csb_hao"] = "HaoCutscene", // ERROR
                ["csb_hugh"] = "HughCutscene",
                // ["csb_imran"] = "ImranCutscene", // ERROR
                // ["csb_janitor"] = "JanitorCutscene", // ERROR
                // ["csb_maude"] = "MaudeCutscene", // ERROR
                ["csb_mweather"] = "MerryWeatherCutscene",
                // ["csb_ortega"] = "OrtegaCutscene", // ERROR
                // ["csb_oscar"] = "OscarCutscene", // ERROR
                ["csb_porndudes"] = "PornDudesCutscene",
                // ["csb_prologuedriver"] = "PrologueDriverCutscene", // ERROR
                ["csb_prolsec"] = "PrologueSec01Cutscene",
                // ["csb_ramp_gang"] = "RampGangCutscene", // ERROR
                // ["csb_ramp_hic"] = "RampHicCutscene", // ERROR
                ["csb_ramp_hipster"] = "RampHipsterCutscene",
                ["csb_ramp_marine"] = "RampMarineCutscene",
                // ["csb_ramp_mex"] = "RampMexCutscene", // ERROR
                ["csb_reporter"] = "ReporterCutscene",
                // ["csb_roccopelosi"] = "RoccoPelosiCutscene", // ERROR
                // ["csb_screen_writer"] = "ScreenWriterCutscene", // ERROR
                // ["csb_stripper_01"] = "Stripper01Cutscene", // ERROR
                // ["csb_stripper_02"] = "Stripper02Cutscene", // ERROR
                ["csb_tonya"] = "TonyaCutscene",
                // ["csb_trafficwarden"] = "TrafficWardenCutscene", // ERROR
                ["g_f_y_ballas_01"] = "Ballas01GFY",
                ["g_f_y_families_01"] = "Families01GFY",
                // ["g_f_y_lost_01"] = "Lost01GFY", // ERROR
                ["g_f_y_vagos_01"] = "Vagos01GFY",
                // ["g_m_m_armboss_01"] = "ArmBoss01GMM", // ERROR
                // ["g_m_m_armgoon_01"] = "ArmGoon01GMM", // ERROR
                // ["g_m_m_armlieut_01"] = "ArmLieut01GMM", // ERROR
                // ["g_m_m_chemwork_01"] = "ChemWork01GMM", // ERROR
                // ["g_m_m_chiboss_01"] = "ChiBoss01GMM", // ERROR
                ["g_m_m_chicold_01"] = "ChiCold01GMM",
                ["g_m_m_chigoon_01"] = "ChiGoon01GMM",
                // ["g_m_m_chigoon_02"] = "ChiGoon02GMM", // ERROR
                ["g_m_m_korboss_01"] = "KorBoss01GMM",
                ["g_m_m_mexboss_01"] = "MexBoss01GMM",
                ["g_m_m_mexboss_02"] = "MexBoss02GMM",
                // ["g_m_y_armgoon_02"] = "ArmGoon02GMY", // ERROR
                ["g_m_y_azteca_01"] = "Azteca01GMY",
                // ["g_m_y_ballaeast_01"] = "BallaEast01GMY", // ERROR
                ["g_m_y_ballaorig_01"] = "BallaOrig01GMY",
                ["g_m_y_ballasout_01"] = "BallaSout01GMY",
                // ["g_m_y_famca_01"] = "Famca01GMY", // ERROR
                // ["g_m_y_famdnf_01"] = "Famdnf01GMY", // ERROR
                // ["g_m_y_famfor_01"] = "Famfor01GMY", // ERROR
                ["g_m_y_korean_01"] = "Korean01GMY",
                // ["g_m_y_korean_02"] = "Korean02GMY", // ERROR
                ["g_m_y_korlieut_01"] = "KorLieut01GMY",
                ["g_m_y_lost_01"] = "Lost01GMY",
                ["g_m_y_lost_02"] = "Lost02GMY",
                ["g_m_y_lost_03"] = "Lost03GMY",
                // ["g_m_y_mexgang_01"] = "MexGang01GMY", // ERROR
                ["g_m_y_mexgoon_01"] = "MexGoon01GMY",
                ["g_m_y_mexgoon_02"] = "MexGoon02GMY",
                // ["g_m_y_mexgoon_03"] = "MexGoon03GMY", // ERROR
                ["g_m_y_pologoon_01"] = "PoloGoon01GMY",
                // ["g_m_y_pologoon_02"] = "PoloGoon02GMY", // ERROR
                // ["g_m_y_salvaboss_01"] = "SalvaBoss01GMY", // ERROR
                ["g_m_y_salvagoon_01"] = "SalvaGoon01GMY",
                ["g_m_y_salvagoon_02"] = "SalvaGoon02GMY",
                ["g_m_y_salvagoon_03"] = "SalvaGoon03GMY",
                // ["g_m_y_strpunk_01"] = "StrPunk01GMY", // ERROR
                ["g_m_y_strpunk_02"] = "StrPunk02GMY",
                ["hc_driver"] = "PestContDriver",
                ["hc_gunman"] = "PestContGunman",
                // ["hc_hacker"] = "Hacker", // ERROR
                ["ig_abigail"] = "Abigail",
                ["ig_amandatownley"] = "AmandaTownley",
                ["ig_andreas"] = "Andreas",
                ["ig_ashley"] = "Ashley",
                // ["ig_ballasog"] = "Ballasog", // ERROR
                // ["ig_bankman"] = "Bankman", // ERROR
                ["ig_barry"] = "Barry",
                ["ig_bestmen"] = "Bestmen",
                // ["ig_beverly"] = "Beverly", // ERROR
                // ["ig_brad"] = "Brad", // ERROR
                ["ig_bride"] = "Bride",
                // ["ig_car3guy1"] = "Car3Guy1", // ERROR
                ["ig_car3guy2"] = "Car3Guy2",
                // ["ig_casey"] = "Casey", // ERROR
                ["ig_chef"] = "Chef",
                // ["ig_chengsr"] = "WeiCheng", // ERROR
                ["ig_chrisformage"] = "CrisFormage",
                // ["ig_claypain"] = "Claypain", // ERROR
                ["ig_clay"] = "Clay",
                // ["ig_cletus"] = "Cletus", // ERROR
                ["ig_dale"] = "Dale",
                ["ig_davenorton"] = "DaveNorton",
                // ["ig_denise"] = "Denise", // ERROR
                ["ig_devin"] = "Devin",
                // ["ig_dom"] = "Dom", // ERROR
                // ["ig_dreyfuss"] = "Dreyfuss", // ERROR
                // ["ig_drfriedlander"] = "DrFriedlander", // ERROR
                // ["ig_fabien"] = "Fabien", // ERROR
                ["ig_fbisuit_01"] = "FbiSuit01",
                // ["ig_floyd"] = "Floyd", // ERROR
                // ["ig_groom"] = "Groom", // ERROR
                ["ig_hao"] = "Hao",
                // ["ig_hunter"] = "Hunter", // ERROR
                ["ig_janet"] = "Janet",
                ["ig_jay_norris"] = "JayNorris",
                ["ig_jewelass"] = "Jewelass",
                // ["ig_jimmyboston"] = "JimmyBoston", // ERROR
                ["ig_jimmydisanto"] = "JimmyDisanto",
                // ["ig_joeminuteman"] = "JoeMinuteman", // ERROR
                // ["ig_johnnyklebitz"] = "JohnnyKlebitz", // ERROR
                // ["ig_josef"] = "Josef", // ERROR
                ["ig_josh"] = "Josh",
                ["ig_kerrymcintosh"] = "KerryMcintosh",
                ["ig_lamardavis"] = "LamarDavis",
                // ["ig_lazlow"] = "Lazlow", // ERROR
                ["ig_lestercrest"] = "LesterCrest",
                ["ig_lifeinvad_01"] = "Lifeinvad01",
                ["ig_lifeinvad_02"] = "Lifeinvad02",
                // ["ig_magenta"] = "Magenta", // ERROR
                // ["ig_manuel"] = "Manuel", // ERROR
                ["ig_marnie"] = "Marnie",
                // ["ig_maryann"] = "MaryAnn", // ERROR
                ["ig_maude"] = "Maude",
                // ["ig_michelle"] = "Michelle", // ERROR
                // ["ig_milton"] = "Milton", // ERROR
                // ["ig_molly"] = "Molly", // ERROR
                // ["ig_mrk"] = "MrK", // ERROR
                ["ig_mrsphillips"] = "MrsPhillips",
                ["ig_mrs_thornhill"] = "MrsThornhill",
                // ["ig_natalia"] = "Natalia", // ERROR
                // ["ig_nervousron"] = "NervousRon", // ERROR
                // ["ig_nigel"] = "Nigel", // ERROR
                ["ig_old_man1a"] = "OldMan1a",
                // ["ig_old_man2"] = "OldMan2", // ERROR
                ["ig_omega"] = "Omega",
                ["ig_oneil"] = "ONeil",
                ["ig_orleans"] = "Orleans",
                ["ig_ortega"] = "Ortega",
                // ["ig_paper"] = "Paper", // ERROR
                // ["ig_patricia"] = "Patricia", // ERROR
                ["ig_priest"] = "Priest",
                ["ig_prolsec_02"] = "PrologueSec02",
                // ["ig_ramp_gang"] = "RampGang", // ERROR
                ["ig_ramp_hic"] = "RampHic",
                // ["ig_ramp_hipster"] = "RampHipster", // ERROR
                // ["ig_ramp_mex"] = "RampMex", // ERROR
                // ["ig_roccopelosi"] = "RoccoPelosi", // ERROR
                ["ig_russiandrunk"] = "RussianDrunk",
                // ["ig_screen_writer"] = "ScreenWriter", // ERROR
                ["ig_siemonyetarian"] = "SiemonYetarian",
                // ["ig_solomon"] = "Solomon", // ERROR
                ["ig_stevehains"] = "SteveHains",
                ["ig_stretch"] = "Stretch",
                // ["ig_talina"] = "Talina", // ERROR
                ["ig_tanisha"] = "Tanisha",
                // ["ig_taocheng"] = "TaoCheng", // ERROR
                ["ig_taostranslator"] = "TaosTranslator",
                // ["ig_tenniscoach"] = "TennisCoach", // ERROR
                ["ig_terry"] = "Terry",
                // ["ig_tomepsilon"] = "TomEpsilon", // ERROR
                // ["ig_tonya"] = "Tonya", // ERROR
                // ["ig_tracydisanto"] = "TracyDisanto", // ERROR
                ["ig_trafficwarden"] = "TrafficWarden",
                ["ig_tylerdix"] = "TylerDixon",
                // ["ig_wade"] = "Wade", // ERROR
                ["ig_zimbor"] = "Zimbor",
                ["mp_f_deadhooker"] = "DeadHooker",
                // ["mp_f_misty_01"] = "Misty01", // ERROR
                ["mp_f_stripperlite"] = "StripperLite",
                ["mp_g_m_pros_01"] = "MPros01",
                // ["mp_m_claude_01"] = "Claude01", // ERROR
                ["mp_m_exarmy_01"] = "ExArmy01",
                ["mp_m_famdd_01"] = "Famdd01",
                ["mp_m_fibsec_01"] = "FibSec01",
                ["mp_m_marston_01"] = "Marston01",
                // ["mp_m_niko_01"] = "Niko01", // ERROR
                ["mp_m_shopkeep_01"] = "ShopKeep01",
                // ["mp_s_m_armoured_01"] = "Armoured01", // ERROR
                ["s_f_m_fembarber"] = "FemBarberSFM",
                // ["s_f_m_maid_01"] = "Maid01SFM", // ERROR
                // ["s_f_m_shop_high"] = "ShopHighSFM", // ERROR
                ["s_f_m_sweatshop_01"] = "Sweatshop01SFM",
                ["s_f_y_airhostess_01"] = "Airhostess01SFY",
                ["s_f_y_bartender_01"] = "Bartender01SFY",
                ["s_f_y_baywatch_01"] = "Baywatch01SFY",
                ["s_f_y_cop_01"] = "Cop01SFY",
                ["s_f_y_factory_01"] = "Factory01SFY",
                ["s_f_y_hooker_01"] = "Hooker01SFY",
                ["s_f_y_hooker_02"] = "Hooker02SFY",
                ["s_f_y_hooker_03"] = "Hooker03SFY",
                // ["s_f_y_migrant_01"] = "Migrant01SFY", // ERROR
                ["s_f_y_movprem_01"] = "MovPrem01SFY",
                // ["s_f_y_ranger_01"] = "Ranger01SFY", // ERROR
                // ["s_f_y_scrubs_01"] = "Scrubs01SFY", // ERROR
                ["s_f_y_sheriff_01"] = "Sheriff01SFY",
                // ["s_f_y_shop_low"] = "ShopLowSFY", // ERROR
                ["s_f_y_shop_mid"] = "ShopMidSFY",
                ["s_f_y_stripperlite"] = "StripperLiteSFY",
                ["s_f_y_stripper_01"] = "Stripper01SFY",
                ["s_f_y_stripper_02"] = "Stripper02SFY",
                // ["s_f_y_sweatshop_01"] = "Sweatshop01SFY", // ERROR
                ["s_m_m_ammucountry"] = "AmmuCountrySMM",
                // ["s_m_m_armoured_01"] = "Armoured01SMM", // ERROR
                ["s_m_m_armoured_02"] = "Armoured02SMM",
                ["s_m_m_autoshop_01"] = "Autoshop01SMM",
                // ["s_m_m_autoshop_02"] = "Autoshop02SMM", // ERROR
                // ["s_m_m_bouncer_01"] = "Bouncer01SMM", // ERROR
                ["s_m_m_chemsec_01"] = "ChemSec01SMM",
                ["s_m_m_ciasec_01"] = "CiaSec01SMM",
                ["s_m_m_cntrybar_01"] = "Cntrybar01SMM",
                ["s_m_m_dockwork_01"] = "Dockwork01SMM",
                // ["s_m_m_doctor_01"] = "Doctor01SMM", // ERROR
                // ["s_m_m_fiboffice_01"] = "FibOffice01SMM", // ERROR
                ["s_m_m_fiboffice_02"] = "FibOffice02SMM",
                // ["s_m_m_gaffer_01"] = "Gaffer01SMM", // ERROR
                ["s_m_m_gardener_01"] = "Gardener01SMM",
                ["s_m_m_gentransport"] = "GentransportSMM",
                ["s_m_m_hairdress_01"] = "Hairdress01SMM",
                // ["s_m_m_highsec_01"] = "Highsec01SMM", // ERROR
                ["s_m_m_highsec_02"] = "Highsec02SMM",
                // ["s_m_m_janitor"] = "JanitorSMM", // ERROR
                // ["s_m_m_lathandy_01"] = "Lathandy01SMM", // ERROR
                // ["s_m_m_lifeinvad_01"] = "Lifeinvad01SMM", // ERROR
                // ["s_m_m_linecook"] = "LinecookSMM", // ERROR
                ["s_m_m_lsmetro_01"] = "Lsmetro01SMM",
                ["s_m_m_mariachi_01"] = "Mariachi01SMM",
                // ["s_m_m_marine_01"] = "Marine01SMM", // ERROR
                // ["s_m_m_marine_02"] = "Marine02SMM", // ERROR
                // ["s_m_m_migrant_01"] = "Migrant01SMM", // ERROR
                ["s_m_m_movalien_01"] = "MovAlien01",
                // ["s_m_m_movprem_01"] = "Movprem01SMM", // ERROR
                // ["s_m_m_movspace_01"] = "Movspace01SMM", // ERROR
                // ["s_m_m_paramedic_01"] = "Paramedic01SMM", // ERROR
                // ["s_m_m_pilot_01"] = "Pilot01SMM", // ERROR
                // ["s_m_m_pilot_02"] = "Pilot02SMM", // ERROR
                ["s_m_m_postal_01"] = "Postal01SMM",
                ["s_m_m_postal_02"] = "Postal02SMM",
                ["s_m_m_prisguard_01"] = "Prisguard01SMM",
                ["s_m_m_scientist_01"] = "Scientist01SMM",
                // ["s_m_m_security_01"] = "Security01SMM", // ERROR
                ["s_m_m_snowcop_01"] = "Snowcop01SMM",
                ["s_m_m_strperf_01"] = "Strperf01SMM",
                ["s_m_m_strpreach_01"] = "Strpreach01SMM",
                // ["s_m_m_strvend_01"] = "Strvend01SMM", // ERROR
                ["s_m_m_trucker_01"] = "Trucker01SMM",
                // ["s_m_m_ups_01"] = "Ups01SMM", // ERROR
                // ["s_m_m_ups_02"] = "Ups02SMM", // ERROR
                // ["s_m_o_busker_01"] = "Busker01SMO", // ERROR
                ["s_m_y_airworker"] = "AirworkerSMY",
                // ["s_m_y_ammucity_01"] = "Ammucity01SMY", // ERROR
                ["s_m_y_armymech_01"] = "Armymech01SMY",
                // ["s_m_y_autopsy_01"] = "Autopsy01SMY", // ERROR
                // ["s_m_y_barman_01"] = "Barman01SMY", // ERROR
                ["s_m_y_baywatch_01"] = "Baywatch01SMY",
                // ["s_m_y_blackops_01"] = "Blackops01SMY", // ERROR
                ["s_m_y_blackops_02"] = "Blackops02SMY",
                // ["s_m_y_busboy_01"] = "Busboy01SMY", // ERROR
                ["s_m_y_chef_01"] = "Chef01SMY",
                ["s_m_y_clown_01"] = "Clown01SMY",
                // ["s_m_y_construct_01"] = "Construct01SMY", // ERROR
                // ["s_m_y_construct_02"] = "Construct02SMY", // ERROR
                ["s_m_y_cop_01"] = "Cop01SMY",
                // ["s_m_y_dealer_01"] = "Dealer01SMY", // ERROR
                // ["s_m_y_devinsec_01"] = "Devinsec01SMY", // ERROR
                // ["s_m_y_dockwork_01"] = "Dockwork01SMY", // ERROR
                ["s_m_y_doorman_01"] = "Doorman01SMY",
                ["s_m_y_dwservice_01"] = "DwService01SMY",
                // ["s_m_y_dwservice_02"] = "DwService02SMY", // ERROR
                ["s_m_y_factory_01"] = "Factory01SMY",
                // ["s_m_y_fireman_01"] = "Fireman01SMY", // ERROR
                // ["s_m_y_garbage"] = "GarbageSMY", // ERROR
                ["s_m_y_grip_01"] = "Grip01SMY",
                ["s_m_y_hwaycop_01"] = "Hwaycop01SMY",
                ["s_m_y_marine_01"] = "Marine01SMY",
                ["s_m_y_marine_02"] = "Marine02SMY",
                ["s_m_y_marine_03"] = "Marine03SMY",
                ["s_m_y_mime"] = "MimeSMY",
                ["s_m_y_pestcont_01"] = "PestCont01SMY",
                // ["s_m_y_pilot_01"] = "Pilot01SMY", // ERROR
                ["s_m_y_prismuscl_01"] = "PrisMuscl01SMY",
                // ["s_m_y_prisoner_01"] = "Prisoner01SMY", // ERROR
                // ["s_m_y_ranger_01"] = "Ranger01SMY", // ERROR
                // ["s_m_y_robber_01"] = "Robber01SMY", // ERROR
                // ["s_m_y_sheriff_01"] = "Sheriff01SMY", // ERROR
                ["s_m_y_shop_mask"] = "ShopMaskSMY",
                // ["s_m_y_strvend_01"] = "Strvend01SMY", // ERROR
                // ["s_m_y_swat_01"] = "Swat01SMY", // ERROR
                // ["s_m_y_uscg_01"] = "Uscg01SMY", // ERROR
                ["s_m_y_valet_01"] = "Valet01SMY",
                // ["s_m_y_waiter_01"] = "Waiter01SMY", // ERROR
                ["s_m_y_winclean_01"] = "WinClean01SMY",
                ["s_m_y_xmech_01"] = "Xmech01SMY",
                // ["s_m_y_xmech_02"] = "Xmech02SMY", // ERROR
                ["u_f_m_corpse_01"] = "Corpse01",
                ["u_f_m_miranda"] = "Miranda",
                // ["u_f_m_promourn_01"] = "PrologueMournFemale01", // ERROR
                ["u_f_o_moviestar"] = "MovieStar",
                // ["u_f_o_prolhost_01"] = "PrologueHostage01", // ERROR
                // ["u_f_y_bikerchic"] = "BikerChic", // ERROR
                // ["u_f_y_comjane"] = "ComJane", // ERROR
                ["u_f_y_corpse_02"] = "Corpse02",
                // ["u_f_y_hotposh_01"] = "Hotposh01", // ERROR
                // ["u_f_y_jewelass_01"] = "Jewelass01", // ERROR
                ["u_f_y_mistress"] = "Mistress",
                ["u_f_y_poppymich"] = "Poppymich",
                // ["u_f_y_princess"] = "Princess", // ERROR
                ["u_f_y_spyactress"] = "SpyActress",
                // ["u_m_m_aldinapoli"] = "AlDiNapoli", // ERROR
                // ["u_m_m_bankman"] = "Bankman01", // ERROR
                ["u_m_m_bikehire_01"] = "BikeHire01",
                ["u_m_m_fibarchitect"] = "FibArchitect",
                ["u_m_m_filmdirector"] = "FilmDirector",
                ["u_m_m_glenstank_01"] = "Glenstank01",
                // ["u_m_m_griff_01"] = "Griff01", // ERROR
                // ["u_m_m_jesus_01"] = "Jesus01", // ERROR
                // ["u_m_m_jewelsec_01"] = "JewelSec01", // ERROR
                // ["u_m_m_jewelthief"] = "JewelThief", // ERROR
                ["u_m_m_markfost"] = "Markfost",
                // ["u_m_m_partytarget"] = "PartyTarget", // ERROR
                ["u_m_m_prolsec_01"] = "PrologueSec01",
                // ["u_m_m_promourn_01"] = "PrologueMournMale01", // ERROR
                ["u_m_m_rivalpap"] = "RivalPaparazzi",
                // ["u_m_m_spyactor"] = "SpyActor", // ERROR
                // ["u_m_m_willyfist"] = "WillyFist", // ERROR
                ["u_m_o_finguru_01"] = "Finguru01",
                // ["u_m_o_taphillbilly"] = "Taphillbilly", // ERROR
                ["u_m_o_tramp_01"] = "Tramp01",
                // ["u_m_y_abner"] = "Abner", // ERROR
                // ["u_m_y_antonb"] = "Antonb", // ERROR
                // ["u_m_y_babyd"] = "Babyd", // ERROR
                ["u_m_y_baygor"] = "Baygor",
                // ["u_m_y_burgerdrug_01"] = "BurgerDrug", // ERROR
                ["u_m_y_chip"] = "Chip",
                ["u_m_y_cyclist_01"] = "Cyclist01",
                // ["u_m_y_fibmugger_01"] = "FibMugger01", // ERROR
                // ["u_m_y_guido_01"] = "Guido01", // ERROR
                // ["u_m_y_gunvend_01"] = "GunVend01", // ERROR
                // ["u_m_y_hippie_01"] = "Hippie01", // ERROR
                ["u_m_y_imporage"] = "Imporage",
                ["u_m_y_justin"] = "Justin",
                // ["u_m_y_mani"] = "Mani", // ERROR
                ["u_m_y_militarybum"] = "MilitaryBum",
                ["u_m_y_paparazzi"] = "Paparazzi",
                ["u_m_y_party_01"] = "Party01",
                // ["u_m_y_pogo_01"] = "Pogo01", // ERROR
                ["u_m_y_prisoner_01"] = "Prisoner01",
                // ["u_m_y_proldriver_01"] = "PrologueDriver", // ERROR
                ["u_m_y_rsranger_01"] = "RsRanger01AMO",
                ["u_m_y_sbike"] = "SbikeAMO",
                // ["u_m_y_staggrm_01"] = "Staggrm01AMO", // ERROR
                // ["u_m_y_tattoo_01"] = "Tattoo01AMO", // ERROR
                // ["u_m_y_zombie_01"] = "Zombie01", // ERROR
            };
        }

        public static string GetHashStringById(int id)
        {
            if (id >= 0 && id < AllPedsDictionary.Count)
            {
                return AllPedsDictionary.ElementAt(id).Key;
            }
            return null;
        }

        public static int GetPedModelMaxId()
        {
            return AllPedsDictionary.Count; // TODO: avaliar se esse menos 1 é valido
        }

        public static Dictionary<string, string> AllPedsDictionary { get; set; }

        private static readonly Dictionary<string, string> MainModels = new Dictionary<string, string>()
        {
            ["player_one"] = "Franklin",
            ["player_two"] = "Trevor",
            ["player_zero"] = "Michael",
            ["mp_f_freemode_01"] = "FreemodeFemale01",
            ["mp_m_freemode_01"] = "FreemodeMale01"
        };
    }
}