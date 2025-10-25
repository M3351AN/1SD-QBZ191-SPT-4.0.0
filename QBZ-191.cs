// 修改说明:
// 1.移植到4.0啦
// 2.能装FHU191的地方，应该就能装FH191才对，反之亦然
//   美帝的枪口你都能兼容，难道还不兼容自家的嘛？
// 3.加了一个191固定照门位，把原来的后照门位放到了导轨上
//   解决在用龙鳞/龙渊护木的时候要用191照门配导轨准星的割裂感
//   然后略微把瞄具位(包括龙鳞/龙渊护木上的MPR45）往前放了亿点腾位置以免穿模
//   可以美滋滋靠照门多吃1人机
//
// 已知问题：
// 1.枪管/火帽/消音器过热不乏红（这不赖我，原版本mod就是这样的，我不会整啊）
// 2.你在导轨照门位装M4提把的话，你再装个镜子那必然穿模的。
// 3.原mod如果有商人和预设之类的。。。那就是我没搬
//
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace qbz191;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "mod.1sd.qbz191";
    public override string Name { get; init; } = "1SD-QBZ191";
    public override string Author { get; init; } = "saintdeer";
    public override List<string>? Contributors { get; init; } = new List<string>(){"Ukia"};
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.1");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = true;
    public override string? License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class QBZ191Mod(
    DatabaseServer databaseServer,
    CustomItemService customItemService
) : IOnLoad
{
    private Dictionary<MongoId, TemplateItem>? _itemDb;

    public Task OnLoad()
    {
        _itemDb = databaseServer.GetTables().Templates.Items;

        CreateWeapon();
        CreateUpperReceiver();
        CreatePistolGrips();
        CreateHandguards();
        CreateMagazines();
        CreateBarrels();
        CreateStocks();
        CreateGasBlocks();
        CreateSights();
        CreateMuzzles();
        CreateRail();
        CreateSuppressor();
        CreateAmmo();

        return Task.CompletedTask;
    }

    private void CreateWeapon()
    {
        var weaponDetails = new NewItemFromCloneDetails
        {
            ItemTplToClone = "618428466ef05c2ce828f218",
            ParentId = "5447b5f14bdc2d61278b4567",
            NewId = "b6c589ec25350853408c15a0",
            FleaPriceRoubles = 60000,
            HandbookPriceRoubles = 36020,
            HandbookParentId = "5b5f78fc86f77409407a7f90",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 5.8x42mm Assault Rifle",
                    ShortName = "QBZ-191",
                    Description = "A neo-generation assault rifle alternates QBZ-95 by Chinese PLA, multiple optimizations have installed into an AR15-liked rifle to fit in uniqued 5.8x42mm cartridges and stated tactical scenes"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 5.8x42mm 突击步枪",
                    ShortName = "QBZ-191",
                    Description = "中国军队用于替代QBZ-95的新一代突击步枪,其在AR-15步枪结构上进行多重优化,使之适应独特的5.8X42mm弹药及设定的战术需求."
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/weapons/q191_58x42_container.bundle",
                    Rcid = ""
                },
                WeapClass = "assaultRifle",
                WeapUseType = "primary",
                AmmoCaliber = "Caliber58x42",
                WeapFireType = new HashSet<string> { "single", "fullauto" },
                SingleFireRate = 450,
                CanQueueSecondShot = true,
                BFirerate = 750,
                Ergonomics = 48,
                Velocity = 0,
                BEffDist = 500,
                BHearDist = 80,
                IsChamberLoad = true,
                ChamberAmmoCount = 1,
                IsBoltCatch = true,
                DefMagType = "c7332f93b18038949ecb1085",
                DefAmmo = "6bf1974e43598ca9672d9380",
                RecoilForceBack = 354,
                RecoilForceUp = 98,
                RecoilAngle = 90,
                RecolDispersion = 10,
                RecoilReturnSpeedHandRotation = 3,
                RecoilCamera = 0.066f,
                HeatFactor = 1.0f,
                CoolFactor = 3.168f,
                HeatFactorByShot = 1.2f,
                AllowJam = true,
                AllowMisfire = true,
                AllowOverheat = true,
                BaseMalfunctionChance = 0.183425f,
                DurabilityBurnRatio = 1.15f,
                RarityPvE = "Rare",
                AdjustCollimatorsToTrajectory = false,
                shotgunDispersion = 0,
                Foldable = false,
                Retractable = false,
                BoltAction = false,
                ManualBoltCatch = false,
                BurstShotsCount = 3,
                NoFiremodeOnBoltcatch = false,
                IsStationaryWeapon = false,
                IsBeltMachineGun = false,
                WithAnimatorAiming = false,
                BlockLeftStance = false,
                CenterOfImpact = 0.01f,
                AimPlane = 0.16f,
                DeviationCurve = 1.35f,
                DeviationMax = 23,
                SightingRange = 100,
                IronSightRange = 100,
                AimSensitivity = 0.65f,
                HipAccuracyRestorationDelay = 0.2f,
                HipAccuracyRestorationSpeed = 7,
                HipInnaccuracyGain = 0.16f,
                MinRepairDegradation = 0,
                MaxRepairDegradation = 0.03f,
                MinRepairKitDegradation = 0,
                MaxRepairKitDegradation = 0.025f,
                RecoilCategoryMultiplierHandRotation = 0.207f,
                RecoilDampingHandRotation = 0.69f,
                RecoilStableIndexShot = 5,
                RecoilPosZMult = 0.6f,
                RecoilReturnPathDampingHandRotation = 0.48f,
                RecoilReturnPathOffsetHandRotation = 0.01f,
                RecoilStableAngleIncreaseStep = 2.4f,
                TacticalReloadFixation = 0.95f,
                CoolFactorGunMods = 1,
                DoubleActionAccuracyPenalty = 1.5f,
                CanPutIntoDuringTheRaid = true,
                CompactHandling = true,
                MustBoltBeOpennedForExternalReload = false,
                MustBoltBeOpennedForInternalReload = false,
                AllowFeed = true,
                AllowSlide = true,
                HeatFactorGun = 1,
                CoolFactorGun = 3.168f,
                IsFlareGun = false,
                IsOneoff = false,
                IsGrenadeLauncher = false,
                SizeReduceRight = 0,
                CameraToWeaponAngleStep = 0.1f,
                CameraSnap = 3.25f,
                ShotsGroupSettings = new List<ShotsGroupSettings>
        {
            new ShotsGroupSettings
            {
                StartShotIndex = 1,
                EndShotIndex = 200,
            }
        },
                Slots =
                [
                    new Slot
            {
                Name = "mod_pistol_grip",
                Id = "618428466ef05c2ce828f21c",
                Parent = "618428466ef05c2ce828f218",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "98f081647a654ca07492ae67",
                                "98f081647a654ca07492cc07"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_magazine",
                Id = "618428466ef05c2ce828f21d",
                Parent = "618428466ef05c2ce828f218",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            AnimationIndex = -1,
                            Filter =
                            [
                                "c7332f93b18038949ecb1085",
                                "6c65bc737ec596d376fe1728",
                                "6c65bc737ec596d376fe1770"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c394bdc2dae468b4577"
            },
            new Slot
            {
                Name = "mod_reciever",
                Id = "618428466ef05c2ce828f21e",
                Parent = "618428466ef05c2ce828f218",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "3b8f80d2f5acca00e5be3455"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_stock_001",
                Id = "618428466ef05c2ce828f21f",
                Parent = "618428466ef05c2ce828f218",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "c084d0bc91c7fa464257d9e8",
                                "fc90d0bc91c7fa464257d9e8",
                                "c084d0bc91c7fa464257dacc",
                                "c084d0bc91c7fa46425703b2"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ],
                Chambers =
                [
                    new Slot
            {
                Name = "patron_in_weapon",
                Id = "618428466ef05c2ce828f221",
                Parent = "618428466ef05c2ce828f218",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Filter =
                            [
                                "6bf1974e43598ca9672d9380",
                                "92c7788a3fb4dfcd7ec7139a",
                                "f5721720fbb4a603ee5c309d"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d4af244bdc2d962f8b4571"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(weaponDetails);
    }

    private void CreateUpperReceiver()
    {
        var upperDetails = new NewItemFromCloneDetails
        {
            ItemTplToClone = "618405198004cc50514c3594",
            ParentId = "55818a304bdc2db5418b457d",
            NewId = "3b8f80d2f5acca00e5be3455",
            FleaPriceRoubles = 25000,
            HandbookPriceRoubles = 23114,
            HandbookParentId = "5b5f764186f77447ec5d7714",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 Upper Receiver",
                    ShortName = "QBZ-191 upper",
                    Description = "An upper receiver for QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 上机匣",
                    ShortName = "QBZ-191 上机匣",
                    Description = "QBZ-191 的上机匣部件"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/reciever_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.39f,
                Width = 2,
                Height = 1,
                BackgroundColor = "blue",
                Ergonomics = 5,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 1,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.9347f,
                CoolFactor = 1.067f,
                Slots =
                [
            new Slot
            {
                Name = "mod_scope",
                Id = "5df8e4080b92095fd441e596",
                Parent = "5df8e4080b92095fd441e594",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57ac965c24597706be5f975c",
                                "57aca93d2459771f2c7e26db",
                                "544a3f024bdc2d1d388b4568",
                                "544a3a774bdc2d3a388b4567",
                                "5d2dc3e548f035404a1a4798",
                                "57adff4f24597737f373b6e6",
                                "5c0517910db83400232ffee5",
                                "591c4efa86f7741030027726",
                                "570fd79bd2720bc7458b4583",
                                "570fd6c2d2720bc6458b457f",
                                "558022b54bdc2dac148b458d",
                                "5c07dd120db834001c39092d",
                                "5c0a2cec0db834001b7ce47d",
                                "58491f3324597764bc48fa02",
                                "584924ec24597768f12ae244",
                                "5b30b0dc5acfc400153b7124",
                                "6165ac8c290d254f5e6b2f6c",
                                "60a23797a37c940de7062d02",
                                "5d2da1e948f035477b1ce2ba",
                                "5c0505e00db834001b735073",
                                "609a63b6e2ff132951242d09",
                                "584984812459776a704a82a6",
                                "59f9d81586f7744c7506ee62",
                                "570fd721d2720bc5458b4596",
                                "57ae0171245977343c27bfcf",
                                "5dfe6104585a0c3e995c7b82",
                                "544a3d0a4bdc2d1b388b4567",
                                "5d1b5e94d7ad1a2b865a96b0",
                                "609bab8b455afd752b2e6138",
                                "58d39d3d86f77445bb794ae7",
                                "616554fe50224f204c1da2aa",
                                "5c7d55f52e221644f31bff6a",
                                "616584766ef05c2ce828ef57",
                                "5b3b6dc75acfc47a8773fb1e",
                                "615d8d878004cc50514c3233",
                                "5b2389515acfc4771e1be0c0",
                                "577d128124597739d65d0e56",
                                "618b9643526131765025ab35",
                                "618bab21526131765025ab3f",
                                "5c86592b2e2216000e69e77c",
                                "5a37ca54c4a282000d72296a",
                                "5d0a29fed7ad1a002769ad08",
                                "5c064c400db834001d23f468",
                                "58d2664f86f7747fec5834f6",
                                "57c69dd424597774c03b7bbc",
                                "5b3b99265acfc4704b4a1afb",
                                "5aa66a9be5b5b0214e506e89",
                                "5aa66c72e5b5b00016327c93",
                                "5c1cdd302e221602b3137250",
                                "61714b2467085e45ef140b2c",
                                "6171407e50224f204c1da3c5",
                                "61713cc4d8e3106d9806c109",
                                "5b31163c5acfc400153b71cb",
                                "5a33b652c4a28232996e407c",
                                "5a33b2c9c4a282000c5a9511",
                                "59db7eed86f77461f8380365",
                                "5a1ead28fcdbcb001912fa9f",
                                "5dff77c759400025ea5150cf",
                                "626bb8532c923541184624b4",
                                "62811f461d5df4475f46a332",
                                "63fc449f5bd61c6cf3784a88",
                                "6477772ea8a38bb2050ed4db",
                                "6478641c19d732620e045e17",
                                "64785e7c19d732620e045e15",
                                "65392f611406374f82152ba5",
                                "653931da5db71d30ab1d6296",
                                "655f13e0a246670fb0373245",
                                "6567e751a715f85433025998",
                            ]
                        }
                    ],
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_handguard",
                Id = "5df8e4080b92095fd441e598",
                Parent = "5df8e4080b92095fd441e594",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "74a2de7548ee8bad03ff18ea",
                                "c71224cf97f6ac5185c10297",
                                "c71224cf97f6ac5185c1e544",
                                "74a2de7548ee8bad03ff1bcc"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_barrel",
                Id = "5df8e4080b92095fd441e598",
                Parent = "5df8e4080b92095fd441e594",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "4027660efe68e9fec3dabfd6",
                                "746748fd271ee1135cd446ae",
                                "746748fd271ee1135cd44dce"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_rear_191", // 这是新加的191固定照门位
                Id = "5fbcc3e4d6fa9c00c571bb5d",
                Parent = "5fbcc3e4d6fa9c00c571bb58",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5bb20e49d4351e3ec0191de0"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_rear", // 这是挪下来的导轨照门位
                Id = "68fc16b6ca7c542a8cede3f0",
                Parent = "3b8f80d2f5acca00e5be3455",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5bb20e49d4351e3bac1212de",
                                "5ba26b17d4351e00367f9bdd",
                                "5dfa3d7ac41b2312ea33362a",
                                "5c1780312e221602b66cc189",
                                "5fb6564947ce63734e3fa1da",
                                "5bc09a18d4351e003562b68e",
                                "5c18b9192e2216398b5a8104",
                                "5fc0fa957283c4046c58147e",
                                "5894a81786f77427140b8347",
                                "55d5f46a4bdc2d1b198b4567",
                                "5ae30bad5acfc400185c2dc4"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(upperDetails);
    }

    private void CreatePistolGrips()
    {
        // 标准握把
        var stdGrip = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649ade84bdc2d1b2b8b4587",
            ParentId = "55818a684bdc2ddd698b456d",
            NewId = "98f081647a654ca07492ae67",
            FleaPriceRoubles = 15000,
            HandbookPriceRoubles = 13514,
            HandbookParentId = "5b5f761f86f774094242f1a1",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 pistol grip",
                    ShortName = "QBZ-191 grip",
                    Description = "Standard pistol grip for QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 手枪式握把",
                    ShortName = "QBZ-191握把",
                    Description = "QBZ-191的标准手枪握把"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/pistolgrip_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.225f,
                Ergonomics = 5,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 1,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0
            }
        };

        customItemService.CreateItemFromClone(stdGrip);

        // 龙鳞握把
        var customGrip = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649ade84bdc2d1b2b8b4587",
            ParentId = "55818a684bdc2ddd698b456d",
            NewId = "98f081647a654ca07492cc07",
            FleaPriceRoubles = 18000,
            HandbookPriceRoubles = 13514,
            HandbookParentId = "5b5f761f86f774094242f1a1",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 Longlin pistol grip",
                    ShortName = "QBZ-191 Longlin grip",
                    Description = "Longlin pistol grip for QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 龙鳞手枪式握把",
                    ShortName = "QBZ-191龙鳞握把",
                    Description = "QBZ-191龙鳞手枪握把"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/pistolgrip_q191_cus.bundle",
                    Rcid = ""
                },
                Weight = 0.225f,
                Ergonomics = 8,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 1,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0
            }
        };

        customItemService.CreateItemFromClone(customGrip);
    }

    private void CreateHandguards()
    {
        // 标准护木
        var stdHandguard = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d3632e4bdc2d972f8b4569",
            ParentId = "55818a104bdc2db9688b4569",
            NewId = "74a2de7548ee8bad03ff18ea",
            FleaPriceRoubles = 26000,
            HandbookPriceRoubles = 25614,
            HandbookParentId = "5b5f75e486f77447ec5d7712",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 standard handguard kit",
                    ShortName = "QBZ-191_STD",
                    Description = "Standard engineering plastic handguard for QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191标准护木",
                    ShortName = "QBZ-191_STD",
                    Description = "由工程塑料制成的标准护木"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/handguard_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.32f,
                Ergonomics = 6,
                Recoil = -1,
                Accuracy = 1,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                ExamineExperience = 3,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 2,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                HeatFactor = 0.965f,
                CoolFactor = 1.03f,
                Slots =
                [
                    new Slot
            {
                Name = "mod_mount_001",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be4575acfc400161d0832"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_000",
                Id = "5df916dfbb49d91fb446d6c2",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5c5952732e2216398b5abda2",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_mount_002",
                Id = "5df916dfbb49d91fb446d6c3",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be4575acfc400161d0832"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_foregrip",
                Id = "5df916dfbb49d91fb446d6c3",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be46e5acfc400170e2dcf"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_front",
                Id = "5d123102d7ad1a004e475feb",
                Parent = "5d123102d7ad1a004e475fe5",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5ba26b01d4351e00818eca51"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(stdHandguard);

        // DMR护木
        var dmrHandguard = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d3632e4bdc2d972f8b4569",
            ParentId = "55818a104bdc2db9688b4569",
            NewId = "74a2de7548ee8bad03ff1bcc",
            FleaPriceRoubles = 44000,
            HandbookPriceRoubles = 38614,
            HandbookParentId = "5b5f75e486f77447ec5d7712",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBU-191 standard handguard kit",
                    ShortName = "QBU-191_STD",
                    Description = "Standard handguard for QBU-191"
                },
                ["ch"] = new()
                {
                    Name = "QBU-191标准护木",
                    ShortName = "QBU-191_STD",
                    Description = "由铝合金制成的标准护木"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/handguard_q191_dmr.bundle",
                    Rcid = ""
                },
                Weight = 0.32f,
                Ergonomics = 5,
                Recoil = -2,
                BackgroundColor = "blue",
                Width = 3,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                ExamineExperience = 3,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 2,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 1,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                HeatFactor = 0.965f,
                CoolFactor = 1.03f,
                Slots =
                [
                    new Slot
            {
                Name = "mod_tactical_001",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5a800961159bd4315e3a1657",
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5cc9c20cd7f00c001336c65d",
                                "5d2369418abbc306c62e0c80",
                                "5b07dd285acfc4001754240d",
                                "56def37dd2720bec348b456a",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5a5f1ce64f39f90b401987bc",
                                "560d657b4bdc2da74d8b4572",
                                "5b3a337e5acfc4704b4a19a0",
                                "5c5952732e2216398b5abda2",
                                "57d17e212459775a1179a0f5",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_000",
                Id = "5df916dfbb49d91fb446d6c2",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5c5952732e2216398b5abda2",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_002",
                Id = "5df916dfbb49d91fb446d6c3",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5a800961159bd4315e3a1657",
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5cc9c20cd7f00c001336c65d",
                                "5d2369418abbc306c62e0c80",
                                "5b07dd285acfc4001754240d",
                                "56def37dd2720bec348b456a",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5a5f1ce64f39f90b401987bc",
                                "560d657b4bdc2da74d8b4572",
                                "5b3a337e5acfc4704b4a19a0",
                                "5c5952732e2216398b5abda2",
                                "57d17e212459775a1179a0f5",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_003",
                Id = "5df916dfbb49d91fb446d6c3",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5a800961159bd4315e3a1657",
                                "5cc9c20cd7f00c001336c65d",
                                "5d2369418abbc306c62e0c80",
                                "5b07dd285acfc4001754240d",
                                "56def37dd2720bec348b456a",
                                "5a7b483fe899ef0016170d15",
                                "5a5f1ce64f39f90b401987bc",
                                "560d657b4bdc2da74d8b4572",
                                "5b3a337e5acfc4704b4a19a0",
                                "57d17e212459775a1179a0f5",
                                "6267c6396b642f77f56f5c1c",
                                "6272370ee4013c5d7e31f418",
                                "6272379924e29f06af4d5ecb",
                                "626becf9582c3e319310b837",
                                "644a3df63b0b6f03e101e065",
                                "646f6322f43d0c5d62063715",
                                "6644920d49817dc7d505ca71"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_foregrip",
                Id = "5df916dfbb49d91fb446d6c3",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "d6abb4b91051aa5a052e28d7"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_front",
                Id = "5d123102d7ad1a004e475feb",
                Parent = "74a2de7548ee8bad03ff1bcc",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5ba26b01d4351e00818eca51"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(dmrHandguard);

        // TacYL 龙鳞II型护木（长款）
        var longlinHandguard = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d3632e4bdc2d972f8b4569",
            ParentId = "55818a104bdc2db9688b4569",
            NewId = "c71224cf97f6ac5185c10297",
            FleaPriceRoubles = 43000,
            HandbookPriceRoubles = 42514,
            HandbookParentId = "5b5f75e486f77447ec5d7712",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "TacYL LongLin II handguard kit",
                    ShortName = "LongLin-II",
                    Description = "Model & Texture by @YLDCMY\n A light-weight handguard compatible with M-LOK rail setions, also improves accuracy with floating barrels"
                },
                ["ch"] = new()
                {
                    Name = "TacYL 龙鳞II型护木",
                    ShortName = "龙鳞-II",
                    Description = "模型及贴图由 @YL永恒梦魇 制作\n 一型轻量化M-LOK护木,同时通过浮置枪管方式提升精度"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/handguard_q191_ll02.bundle",
                    Rcid = ""
                },
                Weight = 0.38f,
                Ergonomics = 10,
                Recoil = -2,
                Accuracy = 4,
                BackgroundColor = "blue",
                Width = 3,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                ExamineExperience = 3,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 2,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                HeatFactor = 0.965f,
                CoolFactor = 1.03f,
                Slots =
                [
                    new Slot
            {
                Name = "mod_scope",
                Id = "5df916dfbb49d91fb446d6bb",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5649a2464bdc2d91118b45a8"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_000",
                Id = "5df916dfbb49d91fb446d6bc",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5c5952732e2216398b5abda2",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_mount_001",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be47f5acfc400170e2dd2",
                                "6269220d70b6c02e665f2635",
                                "6269545d0e57f218e4548ca2"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_mount_002",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be47f5acfc400170e2dd2",
                                "6269220d70b6c02e665f2635",
                                "6269545d0e57f218e4548ca2"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_foregrip",
                Id = "5df916dfbb49d91fb446d6c2",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57cffb66245977632f391a99",
                                "57cffcd624597763133760c5",
                                "57cffcdd24597763f5110006",
                                "57cffce524597763b31685d8",
                                "5b7be4895acfc400170e2dd5",
                                "651a8bf3a8520e48047bf708",
                                "651a8e529829226ceb67c319"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_front",
                Id = "5d123102d7ad1a004e475feb",
                Parent = "5d123102d7ad1a004e475fe5",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5ba26b01d4351e0085325a51",
                                "5dfa3d950dee1b22f862eae0",
                                "5c17804b2e2216152006c02f",
                                "5fb6567747ce63734e3fa1dc",
                                "5bc09a30d4351e00367fb7c8",
                                "5c18b90d2e2216152142466b",
                                "5fc0fa362770a0045c59c677",
                                "5894a73486f77426d259076c"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(longlinHandguard);

        // TacYL 龙渊II型护木（短款）
        var longyuanHandguard = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d3632e4bdc2d972f8b4569",
            ParentId = "55818a104bdc2db9688b4569",
            NewId = "c71224cf97f6ac5185c1e544",
            FleaPriceRoubles = 39000,
            HandbookPriceRoubles = 38514,
            HandbookParentId = "5b5f75e486f77447ec5d7712",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "TacYL LongYuan II handguard kit",
                    ShortName = "LongYuan-II",
                    Description = "Model & Texture by @YLDCMY\n A light-weight handguard compatible with M-LOK rail setions, also improves accuracy with floating barrels"
                },
                ["ch"] = new()
                {
                    Name = "TacYL 龙渊II型护木",
                    ShortName = "龙渊-II",
                    Description = "模型及贴图由 @YL永恒梦魇 制作\n 一型轻量化M-LOK护木,同时通过浮置枪管方式提升精度"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/handguard_q191_ly02.bundle",
                    Rcid = ""
                },
                Weight = 0.38f,
                Ergonomics = 8,
                Recoil = -1,
                Accuracy = 4,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                ExamineExperience = 3,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 2,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                HeatFactor = 0.965f,
                CoolFactor = 1.03f,
                Slots =
                [
                    new Slot
            {
                Name = "mod_scope",
                Id = "5df916dfbb49d91fb446d6bb",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5649a2464bdc2d91118b45a8"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_tactical_000",
                Id = "5df916dfbb49d91fb446d6bc",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57fd23e32459772d0805bcf1",
                                "544909bb4bdc2d6f028b4577",
                                "5d10b49bd7ad1a1a560708b0",
                                "5c06595c0db834001a66af6c",
                                "5a7b483fe899ef0016170d15",
                                "61605d88ffa6e502ac5e7eeb",
                                "5c5952732e2216398b5abda2",
                                "644a3df63b0b6f03e101e065"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_mount_001",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be47f5acfc400170e2dd2",
                                "6269220d70b6c02e665f2635",
                                "6269545d0e57f218e4548ca2"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_mount_002",
                Id = "5df916dfbb49d91fb446d6bd",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b7be47f5acfc400170e2dd2",
                                "6269220d70b6c02e665f2635",
                                "6269545d0e57f218e4548ca2"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_foregrip",
                Id = "5df916dfbb49d91fb446d6c2",
                Parent = "5df916dfbb49d91fb446d6b9",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "57cffb66245977632f391a99",
                                "57cffcd624597763133760c5",
                                "57cffcdd24597763f5110006",
                                "57cffce524597763b31685d8",
                                "5b7be4895acfc400170e2dd5",
                                "651a8bf3a8520e48047bf708",
                                "651a8e529829226ceb67c319"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_sight_front",
                Id = "5d123102d7ad1a004e475feb",
                Parent = "5d123102d7ad1a004e475fe5",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5ba26b01d4351e0085325a51",
                                "5dfa3d950dee1b22f862eae0",
                                "5c17804b2e2216152006c02f",
                                "5fb6567747ce63734e3fa1dc",
                                "5bc09a30d4351e00367fb7c8",
                                "5c18b90d2e2216152142466b",
                                "5fc0fa362770a0045c59c677",
                                "5894a73486f77426d259076c"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(longyuanHandguard);
    }

    private void CreateMagazines()
    {
        // 30发标准弹匣
        var stdMag = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d482194bdc2d1d4e8b456b",
            ParentId = "5448bc234bdc2d3c308b4569",
            NewId = "c7332f93b18038949ecb1085",
            FleaPriceRoubles = 6100,
            HandbookPriceRoubles = 5930,
            HandbookParentId = "5b5f754a86f774094242f19b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 30-rounds 5.8x42mm standard mag",
                    ShortName = "QBZ-191 std",
                    Description = "An 30-rounds 5.8x42mm standard magazine for assault rifles & machineguns"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 30发标准弹匣",
                    ShortName = "QBZ-191 std",
                    Description = "突击步枪与机枪适用的30发5.8x42mm标准弹匣"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/magazine_q191_30r.bundle",
                    Rcid = ""
                },
                Weight = 0.145f,
                Ergonomics = -3,
                BackgroundColor = "yellow",
                Width = 1,
                Height = 2,
                StackMaxSize = 1,
                RarityPvE = "Common",
                ItemSound = "mag_plastic",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 1,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = false,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                CanFast = true,
                CanHit = false,
                CanAdmin = false,
                LoadUnloadModifier = 0,
                CheckTimeModifier = 0,
                CheckOverride = -10,
                VisibleAmmoRangesString = "1-3",
                MalfunctionChance = 0.098f,
                TagColor = 0,
                TagName = "",
                MagazineWithBelt = false,
                BeltMagazineRefreshCount = 0,
                IsMagazineForStationaryWeapon = false,
                Cartridges =
                [
                    new Slot
            {
                Name = "cartridges",
                Id = "5ac66c5d5acfc4001718d316",
                Parent = "5ac66c5d5acfc4001718d314",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "6bf1974e43598ca9672d9380",
                                "92c7788a3fb4dfcd7ec7139a",
                                "f5721720fbb4a603ee5c309d"
                            ]
                        }
                    ]
                },
                MaxCount = 30,
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "5748538b2459770af276a261"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(stdMag);

        // 40发扩容弹匣
        var extMag = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d482194bdc2d1d4e8b456b",
            ParentId = "5448bc234bdc2d3c308b4569",
            NewId = "6c65bc737ec596d376fe1728",
            FleaPriceRoubles = 25000,
            HandbookPriceRoubles = 23930,
            HandbookParentId = "5b5f754a86f774094242f19b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 40-rounds 5.8x42mm standard mag",
                    ShortName = "QBZ-191 ext",
                    Description = "An 40-rounds 5.8x42mm extend magazine for assault rifles & machineguns"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 40发扩容弹匣",
                    ShortName = "QBZ-191 ext",
                    Description = "突击步枪与机枪适用的40发5.8x42mm扩容弹匣"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/magazine_q191_40r.bundle",
                    Rcid = ""
                },
                Weight = 0.18f,
                Ergonomics = -5,
                BackgroundColor = "yellow",
                Width = 1,
                Height = 3,
                StackMaxSize = 1,
                RarityPvE = "Common",
                ItemSound = "mag_plastic",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 2,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = false,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                CanFast = true,
                CanHit = false,
                CanAdmin = false,
                LoadUnloadModifier = 0,
                CheckTimeModifier = 15,
                CheckOverride = 0,
                VisibleAmmoRangesString = "1-3",
                MalfunctionChance = 0.098f,
                TagColor = 0,
                TagName = "",
                MagazineWithBelt = false,
                BeltMagazineRefreshCount = 0,
                IsMagazineForStationaryWeapon = false,
                Cartridges =
                [
                    new Slot
            {
                Name = "cartridges",
                Id = "5ac66c5d5acfc4001718d316",
                Parent = "5ac66c5d5acfc4001718d314",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "6bf1974e43598ca9672d9380",
                                "92c7788a3fb4dfcd7ec7139a",
                                "f5721720fbb4a603ee5c309d"
                            ]
                        }
                    ]
                },
                MaxCount = 40,
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "5748538b2459770af276a261"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(extMag);

        // 60发扩容弹匣
        var ext60Mag = new NewItemFromCloneDetails
        {
            ItemTplToClone = "55d482194bdc2d1d4e8b456b",
            ParentId = "5448bc234bdc2d3c308b4569",
            NewId = "6c65bc737ec596d376fe1770",
            FleaPriceRoubles = 28000,
            HandbookPriceRoubles = 23930,
            HandbookParentId = "5b5f754a86f774094242f19b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 60-rounds 5.8x42mm OEM mag",
                    ShortName = "QBZ-191 60R",
                    Description = "An 60-rounds 5.8x42mm post-market magazine for assault rifles & machineguns"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 60发扩容弹匣",
                    ShortName = "QBZ-191 60R",
                    Description = "突击步枪与机枪适用的60发5.8x42mm售后型扩容弹匣"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/magazine_q191_60r.bundle",
                    Rcid = ""
                },
                Weight = 0.18f,
                Ergonomics = -5,
                BackgroundColor = "yellow",
                Width = 1,
                Height = 2,
                StackMaxSize = 1,
                RarityPvE = "Common",
                ItemSound = "mag_plastic",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 1,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = false,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                CanFast = true,
                CanHit = false,
                CanAdmin = false,
                LoadUnloadModifier = 0,
                CheckTimeModifier = 15,
                CheckOverride = 0,
                VisibleAmmoRangesString = "1-3",
                MalfunctionChance = 0.098f,
                TagColor = 0,
                TagName = "",
                MagazineWithBelt = false,
                BeltMagazineRefreshCount = 0,
                IsMagazineForStationaryWeapon = false,
                Cartridges =
                [
                    new Slot
            {
                Name = "cartridges",
                Id = "5ac66c5d5acfc4001718d316",
                Parent = "5ac66c5d5acfc4001718d314",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "6bf1974e43598ca9672d9380",
                                "92c7788a3fb4dfcd7ec7139a",
                                "f5721720fbb4a603ee5c309d"
                            ]
                        }
                    ]
                },
                MaxCount = 60,
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "5748538b2459770af276a261"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(ext60Mag);
    }

    private void CreateBarrels()
    {
        // 270mm短枪管
        var shortBarrel = new NewItemFromCloneDetails
        {
            ItemTplToClone = "6183fd911cb55961fa0fdce9",
            ParentId = "555ef6e44bdc2de9068b457e",
            NewId = "4027660efe68e9fec3dabfd6",
            FleaPriceRoubles = 25000,
            HandbookPriceRoubles = 21563,
            HandbookParentId = "5b5f75c686f774094242f19f",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-192 270mm barrel",
                    ShortName = "QBZ-192 270mm",
                    Description = "A barrel in 270mm compatibled with QBZ-191/192"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-192 270mm 短枪管",
                    ShortName = "QBZ-192 270mm",
                    Description = "QBZ-191/192适用的270mm枪管"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/barrel_q191_270mm.bundle",
                    Rcid = ""
                },
                Weight = 0.86f,
                Ergonomics = -6,
                Recoil = -3,
                Velocity = -7.36f,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                CenterOfImpact = 0.050f,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.93f,
                CoolFactor = 0.98f,
                ExtraSizeLeft = 2,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 6,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Slots =
                [
                    new Slot
            {
                Name = "mod_muzzle",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b3a16655acfc40016387a2a",
                                "5c7e5f112e221600106f4ede",
                                "5c0fafb6d174af02a96260ba",
                                "612e0cfc8004cc50514c2d9e",
                                "5cf6937cd7f00c056c53fb39",
                                "544a38634bdc2d58388b4568",
                                "5cff9e5ed7ad1a09407397d4",
                                "5c48a2a42e221602b66d1e07",
                                "5f6372e2865db925d54f3869",
                                "615d8e2f1cb55961fa0fd9a4",
                                "56ea8180d2720bf2698b456a",
                                "5d02676dd7ad1a049e54f6dc",
                                "56ea6fafd2720b844b8b4593",
                                "5943ee5a86f77413872d25ec",
                                "609269c3b0e443224b421cc1",
                                "5c7fb51d2e2216001219ce11",
                                "5ea172e498dacb342978818e",
                                "5c6d710d2e22165df16b81e7",
                                "612e0e55a112697a4b3a66e7",
                                "5d440625a4b9361eec4ae6c5",
                                "5cc9b815d7f00c000e2579d6",
                                "5a7c147ce899ef00150bd8b8",
                                "5c7954d52e221600106f4cc7",
                                "59bffc1f86f77435b128b872",
                                "5a9fbb84a2750c00137fa685",
                                "626667e87379c44d557b7550",
                                "62669bccdb9ebb4daa44cd14",
                                "626a74340be03179a165e30c",
                                "6386120cd6baa055ad1e201c",
                                "63ac5c9658d0485fc039f0b8",
                                "6405ff6bd4578826ec3e377a",
                                "626667e87379c44d557be663",
                                "626667e87379c44d557be773"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_gas_block",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "9ec2369685fd526b824fe2a1"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(shortBarrel);

        // 370mm标准枪管
        var stdBarrel = new NewItemFromCloneDetails
        {
            ItemTplToClone = "6183b0711cb55961fa0fdcad",
            ParentId = "555ef6e44bdc2de9068b457e",
            NewId = "746748fd271ee1135cd446ae",
            FleaPriceRoubles = 25000,
            HandbookPriceRoubles = 32563,
            HandbookParentId = "5b5f75c686f774094242f19f",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 370mm barrel",
                    ShortName = "QBZ-191 370mm",
                    Description = "A barrel in 370mm compatibled with QBZ-191/192"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 370mm 枪管",
                    ShortName = "QBZ-191 370mm",
                    Description = "QBZ-191/192适用的370mm枪管"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/barrel_q191_370mm.bundle",
                    Rcid = ""
                },
                Weight = 0.98f,
                Ergonomics = -10,
                Recoil = -3,
                Velocity = -5.21f,
                BackgroundColor = "blue",
                Width = 3,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                CenterOfImpact = 0.046f,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.93f,
                CoolFactor = 0.98f,
                ExtraSizeLeft = 3,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 6,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Slots =
                [
                    new Slot
            {
                Name = "mod_muzzle",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b3a16655acfc40016387a2a",
                                "5c7e5f112e221600106f4ede",
                                "5c0fafb6d174af02a96260ba",
                                "612e0cfc8004cc50514c2d9e",
                                "5cf6937cd7f00c056c53fb39",
                                "544a38634bdc2d58388b4568",
                                "5cff9e5ed7ad1a09407397d4",
                                "5c48a2a42e221602b66d1e07",
                                "5f6372e2865db925d54f3869",
                                "615d8e2f1cb55961fa0fd9a4",
                                "56ea8180d2720bf2698b456a",
                                "5d02676dd7ad1a049e54f6dc",
                                "56ea6fafd2720b844b8b4593",
                                "5943ee5a86f77413872d25ec",
                                "609269c3b0e443224b421cc1",
                                "5c7fb51d2e2216001219ce11",
                                "5ea172e498dacb342978818e",
                                "5c6d710d2e22165df16b81e7",
                                "612e0e55a112697a4b3a66e7",
                                "5d440625a4b9361eec4ae6c5",
                                "5cc9b815d7f00c000e2579d6",
                                "5a7c147ce899ef00150bd8b8",
                                "5c7954d52e221600106f4cc7",
                                "59bffc1f86f77435b128b872",
                                "5a9fbb84a2750c00137fa685",
                                "626667e87379c44d557b7550",
                                "62669bccdb9ebb4daa44cd14",
                                "626a74340be03179a165e30c",
                                "6386120cd6baa055ad1e201c",
                                "63ac5c9658d0485fc039f0b8",
                                "6405ff6bd4578826ec3e377a",
                                "626667e87379c44d557be663",
                                "626667e87379c44d557be773"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_gas_block",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "9ec2369685fd526b824fe2a1"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(stdBarrel);

        // QBU-191 500mm枪管
        var dmrBarrel = new NewItemFromCloneDetails
        {
            ItemTplToClone = "6183b0711cb55961fa0fdcad",
            ParentId = "555ef6e44bdc2de9068b457e",
            NewId = "746748fd271ee1135cd44dce",
            FleaPriceRoubles = 43000,
            HandbookPriceRoubles = 42563,
            HandbookParentId = "5b5f75c686f774094242f19f",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBU-191 500mm barrel",
                    ShortName = "QBU-191 500mm",
                    Description = "A barrel in 500mm compatibled with QBU-191"
                },
                ["ch"] = new()
                {
                    Name = "QBU-191 500mm 枪管",
                    ShortName = "QBU-191 500mm",
                    Description = "QBU-191适用的500mm枪管"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/barrel_q191_dmr.bundle",
                    Rcid = ""
                },
                Weight = 0.98f,
                Ergonomics = -18,
                Recoil = -5,
                Velocity = -2.21f,
                BackgroundColor = "blue",
                Width = 3,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                CenterOfImpact = 0.031f,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.93f,
                CoolFactor = 0.98f,
                ExtraSizeLeft = 4,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 6,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 15,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                ConflictingItems = ["74a2de7548ee8bad03ff18ea", "c71224cf97f6ac5185c1e544"],
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Slots =
                [
                    new Slot
            {
                Name = "mod_muzzle",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5b3a16655acfc40016387a2a",
                                "5c7e5f112e221600106f4ede",
                                "5c0fafb6d174af02a96260ba",
                                "612e0cfc8004cc50514c2d9e",
                                "5cf6937cd7f00c056c53fb39",
                                "544a38634bdc2d58388b4568",
                                "5cff9e5ed7ad1a09407397d4",
                                "5c48a2a42e221602b66d1e07",
                                "5f6372e2865db925d54f3869",
                                "615d8e2f1cb55961fa0fd9a4",
                                "56ea8180d2720bf2698b456a",
                                "5d02676dd7ad1a049e54f6dc",
                                "56ea6fafd2720b844b8b4593",
                                "5943ee5a86f77413872d25ec",
                                "609269c3b0e443224b421cc1",
                                "5c7fb51d2e2216001219ce11",
                                "5ea172e498dacb342978818e",
                                "5c6d710d2e22165df16b81e7",
                                "612e0e55a112697a4b3a66e7",
                                "5d440625a4b9361eec4ae6c5",
                                "5cc9b815d7f00c000e2579d6",
                                "5a7c147ce899ef00150bd8b8",
                                "5c7954d52e221600106f4cc7",
                                "59bffc1f86f77435b128b872",
                                "5a9fbb84a2750c00137fa685",
                                "626667e87379c44d557b7550",
                                "62669bccdb9ebb4daa44cd14",
                                "626a74340be03179a165e30c",
                                "6386120cd6baa055ad1e201c",
                                "63ac5c9658d0485fc039f0b8",
                                "6405ff6bd4578826ec3e377a",
                                "626667e87379c44d557be663",
                                "626667e87379c44d557be773"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            },
            new Slot
            {
                Name = "mod_gas_block",
                Id = "5df917564a9f347bc92edca5",
                Parent = "5df917564a9f347bc92edca3",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "9ec2369685fd526b824fe2cc"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(dmrBarrel);
    }

    private void CreateStocks()
    {
        // AR15兼容缓冲管
        var bufferTube = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649b0fc4bdc2d17108b4588",
            ParentId = "55818a594bdc2db9688b456a",
            NewId = "c084d0bc91c7fa464257d9e8",
            FleaPriceRoubles = 16000,
            HandbookPriceRoubles = 15624,
            HandbookParentId = "5b5f757486f774093e6cb507",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 AR15 compatible buffer tube",
                    ShortName = "AR15 TUBE",
                    Description = "Buffer tube for QBZ-191/192 compatibled with AR15 stock sections"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 AR15 兼容型缓冲管",
                    ShortName = "AR15 TUBE",
                    Description = "一型兼容AR15枪托的QBZ-191/192缓冲管"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/buffer_tube_q191_ar15.bundle",
                    Rcid = ""
                },
                Weight = 0.462f,
                Ergonomics = 1,
                Recoil = -2,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = true,
                HasShoulderContact = true,
                IsShoulderContact = true,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                Foldable = false,
                Retractable = false,
                SizeReduceRight = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 1,
                CoolFactor = 1,
                ExtraSizeRight = 1,
                ExtraSizeLeft = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 2,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Slots =
                [
                    new Slot
            {
                Name = "mod_stock_000",
                Id = "5c793fb92e221644f31bfb66",
                Parent = "5c793fb92e221644f31bfb64",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5fc2369685fd526b824a5713",
                                "606587d11246154cad35d635",
                                "602e620f9b513876d4338d9a",
                                "5a9eb32da2750c00171b3f9c",
                                "5bfe86df0db834001b734685",
                                "55d4ae6c4bdc2d8b2f8b456e",
                                "5c87a07c2e2216001219d4a2",
                                "5bb20e70d4351e0035629f8f",
                                "5beec8c20db834001d2c465c",
                                "5fbbaa86f9986c4cff3fe5f6",
                                "5fce16961f152d4312622bc9",
                                "5ae30c9a5acfc408fb139a03",
                                "5d135e83d7ad1a21b83f42d8",
                                "5d135ecbd7ad1a21c176542e",
                                "56eabf3bd2720b75698b4569",
                                "58d2946386f774496974c37e",
                                "58d2946c86f7744e271174b5",
                                "58d2947686f774485c6a1ee5",
                                "58d2947e86f77447aa070d53",
                                "5d44069ca4b9361ebd26fc37",
                                "5d4406a8a4b9361e4f6eb8b7",
                                "5947c73886f7747701588af5",
                                "5c793fde2e221601da358614",
                                "5b39f8db5acfc40016387a1b",
                                "628a85ee6b1d481ff772e9d5",
                                "6516e91f609aaf354b34b3e2",
                                "6516e971a3d4c6497930b450",
                                "6529370c405a5f51dd023db8"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(bufferTube);

        // QBZ-191 AR15缓冲管基座
        var bufferTubeAdapter = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649b0fc4bdc2d17108b4588",
            ParentId = "55818a594bdc2db9688b456a",
            NewId = "c084d0bc91c7fa464257dacc",
            FleaPriceRoubles = 5000,
            HandbookPriceRoubles = 4624,
            HandbookParentId = "5b5f757486f774093e6cb507",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 AR15 buffer tube adapter",
                    ShortName = "TUBE ADAPTER",
                    Description = "Buffer tube adapter for QBZ-191/192 compatibled with AR15 stock sections"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 AR15 缓冲管基座",
                    ShortName = "TUBE ADAPTER",
                    Description = "一型兼容AR15缓冲管QBZ-191/192枪托适配器"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/buffer_tube_q191_ar15_02.bundle",
                    Rcid = ""
                },
                Weight = 0.462f,
                Ergonomics = 1,
                Recoil = -1,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = true,
                HasShoulderContact = true,
                IsShoulderContact = true,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                Foldable = false,
                Retractable = false,
                SizeReduceRight = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 1,
                CoolFactor = 1,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 2,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Slots =
                [
                    new Slot
            {
                Name = "mod_stock",
                Id = "5c793fb92e221644f31bfb66",
                Parent = "5c793fb92e221644f31bfb64",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "5a33ca0fc4a282000d72292f",
                                "5c0faeddd174af02a962601f",
                                "5649be884bdc2d79388b4577",
                                "5d120a10d7ad1a4e1026ba85",
                                "5b0800175acfc400153aebd4",
                                "5947e98b86f774778f1448bc",
                                "5947eab886f77475961d96c5",
                                "602e3f1254072b51b239f713",
                                "5c793fb92e221644f31bfb64",
                                "5c793fc42e221600114ca25d",
                                "591aef7986f774139d495f03",
                                "591af10186f774139d495f0e",
                                "627254cc9c563e6e442c398f",
                                "638de3603a1a4031d8260b8c",
                                "fc90d0bc91c7fa46425e03b2",
                                "fc90d0bc91c7fa46425e0330"
                            ]
                        }
                    ]
                },
                Required = true,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(bufferTubeAdapter);

        // QBZ-191 QT-A枪托
        var qtaStock = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649b0fc4bdc2d17108b4588",
            ParentId = "55818a594bdc2db9688b456a",
            NewId = "c084d0bc91c7fa46425703b2",
            FleaPriceRoubles = 35000,
            HandbookPriceRoubles = 34624,
            HandbookParentId = "5b5f757486f774093e6cb507",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 QT-A stock",
                    ShortName = "QT-A stock",
                    Description = "QT-A stock for QBZ-191/192"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 QT-A 枪托",
                    ShortName = "QT-A 枪托",
                    Description = "QBZ-191/192 QT-A枪托"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/stock_q191_acr.bundle",
                    Rcid = ""
                },
                Weight = 0.462f,
                Ergonomics = 18,
                Recoil = -26,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = true,
                HasShoulderContact = true,
                IsShoulderContact = true,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                Foldable = false,
                Retractable = false,
                SizeReduceRight = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 1,
                CoolFactor = 1,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 1,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 2,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true
            }
        };

        customItemService.CreateItemFromClone(qtaStock);

        // 标准枪托
        var stdStock = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5649b0fc4bdc2d17108b4588",
            ParentId = "55818a594bdc2db9688b456a",
            NewId = "5fc2369685fd526b824fe2a1",
            FleaPriceRoubles = 19000,
            HandbookPriceRoubles = 18624,
            HandbookParentId = "5b5f757486f774093e6cb507",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 standard stock",
                    ShortName = "QBZ STOCK",
                    Description = "Stock for QBZ-191/192 in standard configuration"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 标准枪托",
                    ShortName = "QBZ STOCK",
                    Description = "QBZ-191/192的标准枪托"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/stock_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.462f,
                Ergonomics = 8,
                Recoil = -23,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = true,
                HasShoulderContact = true,
                IsShoulderContact = true,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                Foldable = false,
                Retractable = false,
                SizeReduceRight = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 1,
                CoolFactor = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 2,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 10,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true
            }
        };

        customItemService.CreateItemFromClone(stdStock);
    }

    private void CreateGasBlocks()
    {
        // 标准导气箍
        var gasBlock = new NewItemFromCloneDetails
        {
            ItemTplToClone = "56ea8d2fd2720b7c698b4570",
            ParentId = "56ea9461d2720b67698b456f",
            NewId = "9ec2369685fd526b824fe2a1",
            FleaPriceRoubles = 7000,
            HandbookPriceRoubles = 6514,
            HandbookParentId = "5b5f760586f774093e6cb509",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 gas block",
                    ShortName = "QBZ-191 GB",
                    Description = "Gas block of QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 导气箍",
                    ShortName = "QBZ-191导气",
                    Description = "QBZ-191专用导气箍"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/gas_block_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.049f,
                Ergonomics = -1,
                Recoil = -4,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "generic",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.995f,
                CoolFactor = 1.004f,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 10,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true
            }
        };

        customItemService.CreateItemFromClone(gasBlock);

        // DMR导气箍
        var dmrGasBlock = new NewItemFromCloneDetails
        {
            ItemTplToClone = "56ea8d2fd2720b7c698b4570",
            ParentId = "56ea9461d2720b67698b456f",
            NewId = "9ec2369685fd526b824fe2cc",
            FleaPriceRoubles = 7000,
            HandbookPriceRoubles = 6514,
            HandbookParentId = "5b5f760586f774093e6cb509",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBU-191 gas block",
                    ShortName = "QBU-191 GB",
                    Description = "Gas block of QBU-191"
                },
                ["ch"] = new()
                {
                    Name = "QBU-191 导气箍",
                    ShortName = "QBU-191导气",
                    Description = "QBU-191专用导气箍"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/gas_block_q191_dmr.bundle",
                    Rcid = ""
                },
                Weight = 0.049f,
                Ergonomics = -1,
                Recoil = -6,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                StackObjectsCount = 1,
                ItemSound = "generic",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Velocity = 0,
                RaidModdable = false,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                DurabilityBurnModificator = 1,
                HeatFactor = 0.995f,
                CoolFactor = 1.004f,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                ExamineExperience = 10,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true
            }
        };

        customItemService.CreateItemFromClone(dmrGasBlock);
    }

    private void CreateSights()
    {
        // 前准星
        var frontSight = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5ae099875acfc4001714e593",
            ParentId = "55818ac54bdc2d5b648b456e",
            NewId = "5ba26b01d4351e00818eca51",
            FleaPriceRoubles = 1200,
            HandbookPriceRoubles = 1195,
            HandbookParentId = "5b5f746686f77447ec5d7708",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 Front Sight",
                    ShortName = "191 Front Sight",
                    Description = "Standard iron sight of QBZ-191"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 准星",
                    ShortName = "191 准星",
                    Description = "QBZ-191的标准机械瞄具"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/sight_front_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.034f,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                RarityPvE = "Not_exist",
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Ergonomics = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 300,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                SightModType = "iron",
                ScopesCount = 1,
            }
        };

        customItemService.CreateItemFromClone(frontSight);

        // QBZ-191后照门
        var rearSight = new NewItemFromCloneDetails
        {
            ItemTplToClone = "5ae099875acfc4001714e593",
            ParentId = "55818ac54bdc2d5b648b456e",
            NewId = "5bb20e49d4351e3ec0191de0",
            FleaPriceRoubles = 1200,
            HandbookPriceRoubles = 1195,
            HandbookParentId = "5b5f746686f77447ec5d7708",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 Rear Sight",
                    ShortName = "191 Rear Sight",
                    Description = "Iron sight for CQB/CQC scenes"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 照门",
                    ShortName = "191 照门",
                    Description = "用于近战的机械瞄具"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/sight_rear_q191_cqb.bundle",
                    Rcid = ""
                },
                Weight = 0.034f,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                RarityPvE = "Not_exist",
                ItemSound = "mod",
                StackObjectsCount = 1,
                NotShownInSlot = false,
                ExaminedByDefault = true,
                ExamineTime = 1,
                IsUndiscardable = false,
                IsUnsaleable = false,
                IsUnbuyable = false,
                IsUngivable = false,
                QuestItem = false,
                LootExperience = 5,
                ExamineExperience = 2,
                HideEntrails = false,
                RepairCost = 0,
                RepairSpeed = 0,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                MergesWithChildren = true,
                CanSellOnRagfair = true,
                CanRequireOnRagfair = false,
                RagFairCommissionModifier = 1,
                IsAlwaysAvailableForInsurance = false,
                DiscardLimit = -1,
                InsuranceDisabled = false,
                QuestStashMaxCount = 0,
                IsSpecialSlotOnly = false,
                CanPutIntoDuringTheRaid = true,
                Durability = 100,
                Accuracy = 0,
                Recoil = 0,
                Loudness = 0,
                EffectiveDistance = 0,
                Ergonomics = 0,
                Velocity = 0,
                RaidModdable = true,
                ToolModdable = true,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 300,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                SightModType = "iron",
                ScopesCount = 1,
                ConflictingItems = // 和导轨上的照门冲突，因为我找不到办法让导轨上有照门的时候让这玩意自动折叠
                [
                                "5bb20e49d4351e3bac1212de",
                                "5ba26b17d4351e00367f9bdd",
                                "5dfa3d7ac41b2312ea33362a",
                                "5c1780312e221602b66cc189",
                                "5fb6564947ce63734e3fa1da",
                                "5bc09a18d4351e003562b68e",
                                "5c18b9192e2216398b5a8104",
                                "5fc0fa957283c4046c58147e",
                                "5894a81786f77427140b8347",
                                "55d5f46a4bdc2d1b198b4567",
                                "5ae30bad5acfc400185c2dc4"
                ]
            }
        };

        customItemService.CreateItemFromClone(rearSight);
    }

    private void CreateMuzzles()
    {
        // 标准消焰器
        var flashHider = new NewItemFromCloneDetails
        {
            ItemTplToClone = "626667e87379c44d557b7550",
            ParentId = "550aa4bf4bdc2dd6348b456b",
            NewId = "626667e87379c44d557be663",
            FleaPriceRoubles = 9000,
            HandbookPriceRoubles = 8000,
            HandbookParentId = "5b5f724c86f774093f2ecf15",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 5.8x42 Flash Hider",
                    ShortName = "FH191",
                    Description = "Flash hider for QBZ-191/192 in standard configuration"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 5.8x42 消焰器",
                    ShortName = "FH191",
                    Description = "QBZ-191/192的标准消焰器"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/muzzle_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.1f,
                Ergonomics = -2,
                Recoil = -5,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                Velocity = 0,
                Slots =
                [
                    new Slot
            {
                Name = "mod_muzzle_001",
                Id = "5fbcc6bd900b1d5091531ddb",
                Parent = "626667e87379c44d557be663",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "70b82ff63184f5af4623a9b8"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(flashHider);

        // QBU-191消焰器
        var flashHiderQBU = new NewItemFromCloneDetails
        {
            ItemTplToClone = "626667e87379c44d557b7550",
            ParentId = "550aa4bf4bdc2dd6348b456b",
            NewId = "626667e87379c44d557be773",
            FleaPriceRoubles = 9000,
            HandbookPriceRoubles = 8000,
            HandbookParentId = "5b5f724c86f774093f2ecf15",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBU-191 5.8x42 Flash Hider",
                    ShortName = "FHU191",
                    Description = "Flash hider for QBU-191 in standard configuration"
                },
                ["ch"] = new()
                {
                    Name = "QBU-191 5.8x42 消焰器",
                    ShortName = "FHU191",
                    Description = "QBU-191的标准消焰器"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/muzzle_q191_dmr.bundle",
                    Rcid = ""
                },
                Weight = 0.1f,
                Ergonomics = -2,
                Recoil = -5,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                Loudness = 0,
                Velocity = 0,
                Slots =
                [
                    new Slot
            {
                Name = "mod_muzzle_001",
                Id = "5fbcc6bd900b1d5091531ddb",
                Parent = "5fbc22ccf24b94483f726483",
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Shift = 0,
                            Filter =
                            [
                                "70b82ff63184f5af4623a9b8"
                            ]
                        }
                    ]
                },
                Required = false,
                MergeSlotWithChildren = false,
                Prototype = "55d30c4c4bdc2db4468b457e"
            }
                ]
            }
        };

        customItemService.CreateItemFromClone(flashHiderQBU);
    }

    private void CreateRail()
    {
        // QBU-191导轨
        var rail = new NewItemFromCloneDetails
        {
            ItemTplToClone = "59e0bed186f774156f04ce84",
            ParentId = "550aa4bf4bdc2dd6348b456b",
            NewId = "d6abb4b91051aa5a052e28d7",
            FleaPriceRoubles = 5000,
            HandbookPriceRoubles = 5000,
            HandbookParentId = "5b5f755f86f77447ec5d770e",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBU-191 rail",
                    ShortName = "QBU191 DG",
                    Description = ""
                },
                ["ch"] = new()
                {
                    Name = "QBU-191 导轨",
                    ShortName = "QBU191 DG",
                    Description = ""
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/mount_q191_dmr_rail.bundle",
                    Rcid = ""
                },
                Weight = 0.1f,
                BackgroundColor = "blue",
                Width = 1,
                Height = 1,
                StackMaxSize = 1,
                ItemSound = "mod",
                Durability = 100
            }
        };

        customItemService.CreateItemFromClone(rail);
    }
    private void CreateSuppressor()
    {
        // 消音器
        var suppressor = new NewItemFromCloneDetails
        {
            ItemTplToClone = "626673016f1edc06f30cf6d5",
            ParentId = "550aa4cd4bdc2dd8348b456c",
            NewId = "70b82ff63184f5af4623a9b8",
            FleaPriceRoubles = 39000,
            HandbookPriceRoubles = 38000,
            HandbookParentId = "5b5f731a86f774093e6cb4f9",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "QBZ-191 5.8x42 Sound Suppressor",
                    ShortName = "Suppressor 191",
                    Description = "Detachable suppressor with 191 flash hider, sacrificed length for better sound suppression"
                },
                ["ch"] = new()
                {
                    Name = "QBZ-191 5.8x42 消音器",
                    ShortName = "Suppressor 191",
                    Description = "191消焰器适用的消音器，牺牲长度以换取更好的消声效果"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/silencers_q191_std.bundle",
                    Rcid = ""
                },
                Weight = 0.453f,
                Ergonomics = -20,
                Recoil = -15,
                Loudness = -34,
                Velocity = 0.6f,
                BackgroundColor = "blue",
                Width = 2,
                Height = 1,
                StackMaxSize = 1,
                RarityPvE = "Superrare",
                ItemSound = "mod",
                Durability = 100,
                Accuracy = 0,
                EffectiveDistance = 0,
                RaidModdable = true,
                ToolModdable = false,
                BlocksFolding = false,
                BlocksCollapsible = false,
                IsAnimated = false,
                HasShoulderContact = false,
                SightingRange = 0,
                DoubleActionAccuracyPenaltyMult = 1,
                UniqueAnimationModID = 0,
                MuzzleModType = "silencer",
                DurabilityBurnModificator = 1.53f,
                HeatFactor = 1.05f,
                CoolFactor = 1.15f
            }
        };

        customItemService.CreateItemFromClone(suppressor);
    }

    private void CreateAmmo()
    {
        // DBP-191弹药
        var ammo191 = new NewItemFromCloneDetails
        {
            ItemTplToClone = "54527ac44bdc2d36668b4567",
            ParentId = "5485a8684bdc2da71d8b4567",
            NewId = "6bf1974e43598ca9672d9380",
            FleaPriceRoubles = 900,
            HandbookPriceRoubles = 870,
            HandbookParentId = "5b47574386f77428ca22b33b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "5.8x42mm DBP-191",
                    ShortName = "DBP-191",
                    Description = "An updated version from 5.8x42mm DBP-10, fast-burning powder made it suited with shorter barrels."
                },
                ["ch"] = new()
                {
                    Name = "5.8x42mm DBP-191",
                    ShortName = "DBP-191",
                    Description = "在5.8x42mm DBP-10弹上进行优化的弹药，通过更换被甲降低了热散影响，速燃发射药的引入也使其更适于较短枪管"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/ammo/dbp191.bundle",
                    Rcid = ""
                },
                Weight = 0.012f,
                Damage = 48,
                PenetrationPower = 48,
                ArmorDamage = 47,
                Caliber = "Caliber58x42",
                InitialSpeed = 940,
                Tracer = false,
                AmmoLifeTimeSec = 5,
                AmmoTooltipClass = "ArmorPiercing",
                AnimationVariantsNumber = 0,
                BackgroundColor = "yellow",
                BulletDiameterMilimeters = 5.7f,
                BulletMassGram = 4f,
                CanRequireOnRagfair = true,
                CanSellOnRagfair = true,
                DurabilityBurnModificator = 1.34f,
                ExamineExperience = 10,
                ExamineTime = 1,
                ExaminedByDefault = true,
                FragmentType = "5996f6d686f77467977ba6cc",
                FragmentationChance = 0.44f,
                FragmentsCount = 0,
                HeatFactor = 1.71f,
                HeavyBleedingDelta = 0,
                Height = 1,
                ItemSound = "ammo_singleround",
                LightBleedingDelta = 0,
                LootExperience = 0,
                MalfFeedChance = 0.094f,
                MalfMisfireChance = 0.175f,
                MaxFragmentsCount = 3,
                MinFragmentsCount = 2,
                MisfireChance = 0.01f,
                PenetrationChanceObstacle = 0.55f,
                PenetrationDamageMod = 0.15f,
                AmmoAccr = -2,
                AmmoRec = 5,
                AmmoDist = 0,
                BuckshotBullets = 0,
                PenetrationPowerDiviation = 1.375f,
                BallisticCoeficient = 0.275f,
                ProjectileCount = 1,
                QuestItem = false,
                RagFairCommissionModifier = 1,
                RarityPvE = "Superrare",
                RemoveShellAfterFire = false,
                RicochetChance = 0.38f,
                ShowBullet = false,
                SpeedRetardation = 0.000023f,
                StackMaxRandom = 40,
                StackMaxSize = 60,
                StackMinRandom = 20,
                StackObjectsCount = 1,
                StaminaBurnPerDamage = 0.144f,
                TracerColor = "red",
                TracerDistance = 0,
                Width = 1,
                AmmoHear = 0,
                AmmoSfx = "standart",
                AmmoShiftChance = 0,
                AmmoType = "bullet",
                CasingEjectPower = 1,
                CasingMass = 11.77f,
                CasingName = "5.56x45 мм M193 FMJ",
                CasingSounds = "rifle556"
            }
        };

        customItemService.CreateItemFromClone(ammo191);

        // DBU-141弹药
        var ammo141 = new NewItemFromCloneDetails
        {
            ItemTplToClone = "61962b617c6c7b169525f168",
            ParentId = "5485a8684bdc2da71d8b4567",
            NewId = "92c7788a3fb4dfcd7ec7139a",
            FleaPriceRoubles = 1200,
            HandbookPriceRoubles = 1200,
            HandbookParentId = "5b47574386f77428ca22b33b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "5.8x42mm DBU-141",
                    ShortName = "DBU-141",
                    Description = "High-accuracy ammo for maskman and sniper rifles, also with better ballistic effects. However, its penetrating power has strong difference with reality,"
                },
                ["ch"] = new()
                {
                    Name = "5.8x42mm DBU-141",
                    ShortName = "DBU-141",
                    Description = "用于射手步枪和狙击步枪的高精度弹药，弹道更为平直且散布较低。但请注意！此弹的穿透表现和现实有大幅差异"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/ammo/dbp141.bundle",
                    Rcid = ""
                },
                Weight = 0.011f,
                Damage = 48,
                PenetrationPower = 42,
                ArmorDamage = 45,
                Caliber = "Caliber58x42",
                InitialSpeed = 935,
                Tracer = false,
                AmmoLifeTimeSec = 5,
                AmmoTooltipClass = "Universal",
                AnimationVariantsNumber = 0,
                BackgroundColor = "yellow",
                BulletDiameterMilimeters = 5.62f,
                BulletMassGram = 3.65f,
                CanRequireOnRagfair = true,
                CanSellOnRagfair = true,
                Description = "patron_545x39_BS",
                Deterioration = 1,
                DiscardLimit = -1,
                DiscardingBlock = false,
                DurabilityBurnModificator = 1.35f,
                ExamineExperience = 100,
                ExamineTime = 1,
                ExaminedByDefault = false,
                ExplosionStrength = 0,
                ExplosionType = "",
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                FlareTypes = new string[0],
                FragmentType = "5996f6d686f77467977ba6cc",
                FragmentationChance = 0.02f,
                FragmentsCount = 0,
                FuzeArmTimeSec = 0,
                HasGrenaderComponent = false,
                HeatFactor = 1.7523f,
                HeavyBleedingDelta = 0.15f,
                Height = 1,
                HideEntrails = false,
                InsuranceDisabled = false,
                IsAlwaysAvailableForInsurance = false,
                IsLightAndSoundShot = false,
                IsSpecialSlotOnly = false,
                IsUnbuyable = false,
                IsUndiscardable = false,
                IsUngivable = false,
                IsUnsaleable = false,
                ItemSound = "ammo_singleround",
                LightAndSoundShotAngle = 0,
                LightAndSoundShotSelfContusionStrength = 0,
                LightAndSoundShotSelfContusionTime = 0,
                LightBleedingDelta = 0.2f,
                LootExperience = 0,
                MalfFeedChance = 0.059f,
                MalfMisfireChance = 0.175f,
                MaxExplosionDistance = 0,
                MaxFragmentsCount = 2,
                MergesWithChildren = false,
                MinExplosionDistance = 0,
                MinFragmentsCount = 1,
                MisfireChance = 0.01f,
                Name = "patron_545x39_BS",
                NotShownInSlot = false,
                PenetrationChanceObstacle = 0.65f,
                PenetrationDamageMod = 0.125f,
                PenetrationPowerDiviation = 1.375f,
                BallisticCoeficient = 0.281f,
                ProjectileCount = 1,
                QuestItem = false,
                QuestStashMaxCount = 0,
                RagFairCommissionModifier = 1,
                RarityPvE = "Superrare",
                RemoveShellAfterFire = false,
                RepairCost = 0,
                RepairSpeed = 0,
                RicochetChance = 0.3f,
                ShortName = "patron_545x39_BS",
                ShowBullet = false,
                ShowHitEffectOnExplode = false,
                SpeedRetardation = 2.5e-05f,
                StackMaxRandom = 30,
                StackMaxSize = 60,
                StackMinRandom = 10,
                StackObjectsCount = 1,
                StaminaBurnPerDamage = 0.1296f,
                TracerColor = "red",
                TracerDistance = 0,
                Unlootable = false,
                UnlootableFromSlot = "FirstPrimaryWeapon",
                UsePrefab = new Prefab
                {
                    Path = "",
                    Rcid = ""
                },
                Width = 1,
                AirDropTemplateId = "",
                AmmoHear = 0,
                AmmoSfx = "standart",
                AmmoShiftChance = 0,
                AmmoType = "bullet",
                CasingEjectPower = 0,
                CasingMass = 16.5f,
                CasingName = "5.45x39 мм 7Н39",
                CasingSounds = "rifle556",
                AmmoAccr = 33,
                AmmoRec = -15,
                AmmoDist = 0,
                BuckshotBullets = 0,

            }
        };

        customItemService.CreateItemFromClone(ammo141);

        // DVC-12弹药
        var ammoDVC12 = new NewItemFromCloneDetails
        {
            ItemTplToClone = "601949593ae8f707c4608daa",
            ParentId = "5485a8684bdc2da71d8b4567",
            NewId = "f5721720fbb4a603ee5c309d",
            FleaPriceRoubles = 1350,
            HandbookPriceRoubles = 1350,
            HandbookParentId = "5b47574386f77428ca22b33b",
            Locales = new Dictionary<string, LocaleDetails>
            {
                ["en"] = new()
                {
                    Name = "5.8x42mm DVC-12",
                    ShortName = "DVC-12",
                    Description = "Armor-piercer with a tungsten cardie core, specific-desighed warhead struture lowers probability of crush when penetrating."
                },
                ["ch"] = new()
                {
                    Name = "5.8x42mm DVC-12",
                    ShortName = "DVC-12",
                    Description = "使用碳化钨弹芯的硬质穿甲弹，经过特殊设计的结构可以降低弹头侵彻时发生碎裂的概率"
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Prefab = new Prefab
                {
                    Path = "assets/q191/ammo/dvc12.bundle",
                    Rcid = ""
                },
                Weight = 0.013f,
                Damage = 38,
                PenetrationPower = 60,
                ArmorDamage = 58,
                Caliber = "Caliber58x42",
                InitialSpeed = 910,
                Tracer = false,
                AmmoLifeTimeSec = 5,
                AmmoTooltipClass = "ArmorPiercing",
                AnimationVariantsNumber = 0,
                BackgroundColor = "yellow",
                BulletDiameterMilimeters = 5.7f,
                BulletMassGram = 3.37f,
                CanRequireOnRagfair = true,
                CanSellOnRagfair = true,
                Deterioration = 1,
                DiscardLimit = -1,
                DiscardingBlock = false,
                DurabilityBurnModificator = 1.8f,
                ExamineExperience = 100,
                ExamineTime = 1,
                ExaminedByDefault = false,
                ExplosionStrength = 0,
                ExplosionType = "",
                ExtraSizeDown = 0,
                ExtraSizeForceAdd = false,
                ExtraSizeLeft = 0,
                ExtraSizeRight = 0,
                ExtraSizeUp = 0,
                FlareTypes = new string[0],
                FragmentType = "5996f6d686f77467977ba6cc",
                FragmentationChance = 0.3f,
                FragmentsCount = 0,
                FuzeArmTimeSec = 0,
                HasGrenaderComponent = false,
                HeatFactor = 1.9f,
                HeavyBleedingDelta = 0,
                Height = 1,
                HideEntrails = false,
                InsuranceDisabled = false,
                IsAlwaysAvailableForInsurance = false,
                IsLightAndSoundShot = false,
                IsSpecialSlotOnly = false,
                IsUnbuyable = false,
                IsUndiscardable = false,
                IsUngivable = false,
                IsUnsaleable = false,
                ItemSound = "ammo_singleround",
                LightAndSoundShotAngle = 0,
                LightAndSoundShotSelfContusionStrength = 0,
                LightAndSoundShotSelfContusionTime = 0,
                LightBleedingDelta = 0,
                LootExperience = 0,
                MalfFeedChance = 0.111f,
                MalfMisfireChance = 0.196f,
                MaxExplosionDistance = 0,
                MaxFragmentsCount = 3,
                MergesWithChildren = false,
                MinExplosionDistance = 0,
                MinFragmentsCount = 2,
                MisfireChance = 0.01f,
                NotShownInSlot = false,
                PenetrationChanceObstacle = 0.7f,
                PenetrationDamageMod = 0.175f,
                AmmoAccr = -4,
                AmmoRec = 12,
                AmmoDist = 0,
                BuckshotBullets = 0,
                PenetrationPowerDiviation = 1.375f,
                BallisticCoeficient = 0.279f,
                ProjectileCount = 1,
                QuestItem = false,
                QuestStashMaxCount = 0,
                RagFairCommissionModifier = 1,
                RarityPvE = "Superrare",
                RemoveShellAfterFire = false,
                RepairCost = 0,
                RepairSpeed = 0,
                RicochetChance = 0.48f,
                ShowBullet = false,
                ShowHitEffectOnExplode = false,
                SpeedRetardation = 2.3e-05f,
                StackMaxRandom = 17,
                StackMaxSize = 60,
                StackMinRandom = 5,
                StackObjectsCount = 1,
                StaminaBurnPerDamage = 0.1152f,
                TracerColor = "red",
                TracerDistance = 0,
                Unlootable = false,
                UnlootableFromSlot = "FirstPrimaryWeapon",
                UsePrefab = new Prefab
                {
                    Path = "",
                    Rcid = ""
                },
                Width = 1,
                AirDropTemplateId = "",
                AmmoHear = 0,
                AmmoSfx = "standart",
                AmmoShiftChance = 0,
                AmmoType = "bullet",
                CasingEjectPower = 1,
                CasingMass = 11.77f,
                CasingName = "5.56x45 мм M193 FMJ",
                CasingSounds = "rifle556"
            }
        };

        customItemService.CreateItemFromClone(ammoDVC12);
    }
}