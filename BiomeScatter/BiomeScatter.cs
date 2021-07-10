using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.Configuration.ModLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Kopernicus;
using Kopernicus.Components.MaterialWrapper;
using Kopernicus.Components.ModularScatter;
using Kopernicus.ConfigParser;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.ConfigParser.Interfaces;
using Kopernicus.Configuration.Enumerations;
using Kopernicus.Configuration.MaterialLoader;
using Kopernicus.Configuration.Parsing;
using Kopernicus.UI;
using UnityEngine;
using System.Linq;

namespace TonyTest
{
    [RequireConfigType(ConfigType.Node)]
    public class BiomeScatter : ModLoader<PQSMod_BiomeScatter>
    {
        // Loader for a Ground Scatter from Kopernicus
        /*[RequireConfigType(ConfigType.Node)]
        public class LandClassScatterLoader : IPatchable, ITypeParser<PQSLandControl.LandClassScatter>
        {
            // The value we are editing
            public PQSLandControl.LandClassScatter Value { get; set; }
            public ModularScatter Scatter { get; set; }

            /// <summary>
            /// Returns the currently edited PQS
            /// </summary>
            protected PQS PqsVersion
            {
                get
                {
                    try
                    {
                        return Parser.GetState<PQS>("Kopernicus:pqsVersion");
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            // Scatter name
            [PreApply]
            [ParserTarget("name")]
            public String name
            {
                get { return Value.scatterName; }
                set { Value.scatterName = value; }
            }

            // Should we delete the Scatter?
            [ParserTarget("delete")]
            [KittopiaHideOption]
            [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
            public NumericParser<Boolean> Delete = false;

            [PreApply]
            [ParserTarget("materialType")]
            public EnumParser<ScatterMaterialType> Type
            {
                get
                {
                    if (NormalDiffuse.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.Diffuse;
                    }
                    if (NormalBumped.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.BumpedDiffuse;
                    }
                    if (NormalDiffuseDetail.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.DiffuseDetail;
                    }
                    if (DiffuseWrap.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.DiffuseWrapped;
                    }
                    if (AlphaTestDiffuse.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.CutoutDiffuse;
                    }
                    if (AerialTransCutout.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.AerialCutout;
                    }
                    if (Standard.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.Standard;
                    }
                    if (StandardSpecular.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.StandardSpecular;
                    }
                    if (KSPBumped.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.KSPBumped;
                    }
                    if (KSPBumpedSpecular.UsesSameShader(Value.material))
                    {
                        return ScatterMaterialType.KSPBumpedSpecular;
                    }

                    throw new Exception("The shader '" + Value.material.shader.name + "' is not supported.");
                }
                set
                {
                    Boolean isDiffuse = NormalDiffuse.UsesSameShader(Value.material);
                    Boolean isBumped = NormalBumped.UsesSameShader(Value.material);
                    Boolean isDetail = NormalDiffuseDetail.UsesSameShader(Value.material);
                    Boolean isWrapped = DiffuseWrap.UsesSameShader(Value.material);
                    Boolean isCutout = AlphaTestDiffuse.UsesSameShader(Value.material);
                    Boolean isAerial = AerialTransCutout.UsesSameShader(Value.material);
                    Boolean isStandard = Standard.UsesSameShader(Value.material);
                    Boolean isStandardSpecular = StandardSpecular.UsesSameShader(Value.material);
                    Boolean isKspBumped = KSPBumped.UsesSameShader(Value.material);
                    Boolean isKspBumpedSpecular = KSPBumpedSpecular.UsesSameShader(Value.material);

                    switch (value.Value)
                    {
                        case ScatterMaterialType.Diffuse when !isDiffuse:
                            Value.material = new NormalDiffuseLoader();
                            break;
                        case ScatterMaterialType.BumpedDiffuse when !isBumped:
                            Value.material = new NormalBumpedLoader();
                            break;
                        case ScatterMaterialType.DiffuseDetail when !isDetail:
                            Value.material = new NormalDiffuseDetailLoader();
                            break;
                        case ScatterMaterialType.DiffuseWrapped when !isWrapped:
                            Value.material = new DiffuseWrapLoader();
                            break;
                        case ScatterMaterialType.CutoutDiffuse when !isCutout:
                            Value.material = new AlphaTestDiffuseLoader();
                            break;
                        case ScatterMaterialType.AerialCutout when !isAerial:
                            Value.material = new AerialTransCutoutLoader();
                            break;
                        case ScatterMaterialType.Standard when !isStandard:
                            Value.material = new StandardLoader();
                            break;
                        case ScatterMaterialType.StandardSpecular when !isStandardSpecular:
                            Value.material = new StandardSpecularLoader();
                            break;
                        case ScatterMaterialType.KSPBumped when !isKspBumped:
                            Value.material = new KSPBumpedLoader();
                            break;
                        case ScatterMaterialType.KSPBumpedSpecular when !isKspBumpedSpecular:
                            Value.material = new KSPBumpedSpecularLoader();
                            break;
                        default:
                            return;
                    }
                }
            }

            // Custom scatter material
            [ParserTarget("Material", AllowMerge = true)]
            public Material Material
            {
                get
                {
                    Boolean isDiffuse = Value.material is NormalDiffuseLoader;
                    Boolean isBumped = Value.material is NormalBumpedLoader;
                    Boolean isDetail = Value.material is NormalDiffuseDetailLoader;
                    Boolean isWrapped = Value.material is DiffuseWrapLoader;
                    Boolean isCutout = Value.material is AlphaTestDiffuseLoader;
                    Boolean isAerial = Value.material is AerialTransCutoutLoader;
                    Boolean isStandard = Value.material is StandardLoader;
                    Boolean isStandardSpecular = Value.material is StandardSpecularLoader;
                    Boolean isKspBumped = Value.material is KSPBumpedLoader;
                    Boolean isKspBumpedSpecular = Value.material is KSPBumpedSpecularLoader;

                    switch (Type.Value)
                    {
                        case ScatterMaterialType.Diffuse when !isDiffuse:
                            Value.material = new NormalDiffuseLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.BumpedDiffuse when !isBumped:
                            Value.material = new NormalBumpedLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.DiffuseDetail when !isDetail:
                            Value.material = new NormalDiffuseDetailLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.DiffuseWrapped when !isWrapped:
                            Value.material = new DiffuseWrapLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.CutoutDiffuse when !isCutout:
                            Value.material = new AlphaTestDiffuseLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.AerialCutout when !isAerial:
                            Value.material = new AerialTransCutoutLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.Standard when !isStandard:
                            Value.material = new StandardLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.StandardSpecular when !isStandardSpecular:
                            Value.material = new StandardSpecularLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.KSPBumped when !isKspBumped:
                            Value.material = new KSPBumpedLoader(Value.material);
                            goto default;
                        case ScatterMaterialType.KSPBumpedSpecular when !isKspBumpedSpecular:
                            Value.material = new KSPBumpedSpecularLoader(Value.material);
                            goto default;
                        default:
                            return Value.material;
                    }
                }
                set
                {
                    Boolean isDiffuse = value is NormalDiffuseLoader;
                    Boolean isBumped = value is NormalBumpedLoader;
                    Boolean isDetail = value is NormalDiffuseDetailLoader;
                    Boolean isWrapped = value is DiffuseWrapLoader;
                    Boolean isCutout = value is AlphaTestDiffuseLoader;
                    Boolean isAerial = value is AerialTransCutoutLoader;
                    Boolean isStandard = value is StandardLoader;
                    Boolean isStandardSpecular = value is StandardSpecularLoader;
                    Boolean isKspBumped = Value.material is KSPBumpedLoader;
                    Boolean isKspBumpedSpecular = Value.material is KSPBumpedSpecularLoader;

                    switch (Type.Value)
                    {
                        case ScatterMaterialType.Diffuse when !isDiffuse:
                            Value.material = new NormalDiffuseLoader(value);
                            break;
                        case ScatterMaterialType.BumpedDiffuse when !isBumped:
                            Value.material = new NormalBumpedLoader(value);
                            break;
                        case ScatterMaterialType.DiffuseDetail when !isDetail:
                            Value.material = new NormalDiffuseDetailLoader(value);
                            break;
                        case ScatterMaterialType.DiffuseWrapped when !isWrapped:
                            Value.material = new DiffuseWrapLoader(value);
                            break;
                        case ScatterMaterialType.CutoutDiffuse when !isCutout:
                            Value.material = new AlphaTestDiffuseLoader(value);
                            break;
                        case ScatterMaterialType.AerialCutout when !isAerial:
                            Value.material = new AerialTransCutoutLoader(value);
                            break;
                        case ScatterMaterialType.Standard when !isStandard:
                            Value.material = new StandardLoader(value);
                            break;
                        case ScatterMaterialType.StandardSpecular when !isStandardSpecular:
                            Value.material = new StandardSpecularLoader(value);
                            break;
                        case ScatterMaterialType.KSPBumped when !isKspBumped:
                            Value.material = new KSPBumpedLoader(value);
                            break;
                        case ScatterMaterialType.KSPBumpedSpecular when !isKspBumpedSpecular:
                            Value.material = new KSPBumpedSpecularLoader(value);
                            break;
                        default:
                            Value.material = value;
                            break;
                    }
                }
            }

            // Stock material
            [ParserTarget("material", AllowMerge = true)]
            public StockMaterialParser StockMaterial
            {
                get { return Material; }
                set { Material = value; }
            }

            // The mesh
            [ParserTarget("mesh")]
            public MeshParser BaseMesh
            {
                get { return Scatter.baseMesh; }
                set { Scatter.baseMesh = value; }
            }

            [ParserTargetCollection("Meshes", AllowMerge = true)]
            public List<MeshParser> Meshes
            {
                get { return Scatter.meshes.Select(m => (MeshParser) m).ToList(); }
                set { Scatter.meshes = value.Select(m => m.Value).ToList(); }
            }

            // castShadows
            [ParserTarget("castShadows")]
            public NumericParser<Boolean> CastShadows
            {
                get { return Value.castShadows; }
                set { Value.castShadows = value; }
            }

            // densityFactor
            [ParserTarget("densityFactor")]
            public NumericParser<Double> DensityFactor
            {
                get { return Value.densityFactor; }
                set { Value.densityFactor = value; }
            }

            // maxCache
            [ParserTarget("maxCache")]
            public NumericParser<Int32> MaxCache
            {
                get { return Value.maxCache; }
                set { Value.maxCache = value; }
            }

            // maxCacheDelta
            [ParserTarget("maxCacheDelta")]
            public NumericParser<Int32> MaxCacheDelta
            {
                get { return Value.maxCacheDelta; }
                set { Value.maxCacheDelta = value; }
            }

            // maxLevelOffset
            [ParserTarget("maxLevelOffset")]
            public NumericParser<Int32> MaxLevelOffset
            {
                get { return Value.maxLevelOffset; }
                set { Value.maxLevelOffset = value; }
            }

            // maxScale
            [ParserTarget("maxScale")]
            public NumericParser<Single> MaxScale
            {
                get { return Value.maxScale; }
                set { Value.maxScale = value; }
            }

            // maxScatter
            [ParserTarget("maxScatter")]
            public NumericParser<Int32> MaxScatter
            {
                get { return Value.maxScatter; }
                set { Value.maxScatter = value; }
            }

            // maxSpeed
            [ParserTarget("maxSpeed")]
            public NumericParser<Double> MaxSpeed
            {
                get { return Value.maxSpeed; }
                set { Value.maxSpeed = value; }
            }

            // minScale
            [ParserTarget("minScale")]
            public NumericParser<Single> MinScale
            {
                get { return Value.minScale; }
                set { Value.minScale = value; }
            }

            // recieveShadows
            [ParserTarget("recieveShadows")]
            public NumericParser<Boolean> RecieveShadows
            {
                get { return Value.recieveShadows; }
                set { Value.recieveShadows = value; }
            }

            // The value we are editing
            // Scatter seed
            [ParserTarget("seed")]
            public NumericParser<Int32> Seed
            {
                get { return Value.seed; }
                set { Value.seed = value; }
            }

            // verticalOffset
            [ParserTarget("verticalOffset")]
            public NumericParser<Single> VerticalOffset
            {
                get { return Value.verticalOffset; }
                set { Value.verticalOffset = value; }
            }

            [ParserTarget("instancing")]
            public NumericParser<Boolean> Instancing
            {
                get { return Value.material.enableInstancing; }
                set { Value.material.enableInstancing = value; }
            }

            [ParserTarget("rotation")]
            public NumericCollectionParser<Single> Rotation
            {
                get { return Scatter.rotation; }
                set { Scatter.rotation = value; }
            }

            [ParserTarget("useBetterDensity")]
            public NumericParser<Boolean> UseBetterDensity
            {
                get { return Scatter.useBetterDensity; }
                set { Scatter.useBetterDensity = value; }
            }

            [ParserTarget("spawnChance")]
            public NumericParser<Single> SpawnChance
            {
                get { return Scatter.spawnChance; }
                set { Scatter.spawnChance = value; }
            }

            [ParserTarget("ignoreDensityGameSetting")]
            public NumericParser<Boolean> IgnoreDensityGameSetting
            {
                get { return Scatter.ignoreDensityGameSetting; }
                set { Scatter.ignoreDensityGameSetting = value; }
            }

            [ParserTarget("densityVariance")]
            public NumericCollectionParser<Single> DensityVariance
            {
                get { return Scatter.densityVariance; }
                set { Scatter.densityVariance = value; }
            }

            [ParserTargetCollection("Components", AllowMerge = true, NameSignificance = NameSignificance.Type)]
            public CallbackList<ComponentLoader<ModularScatter>> Components { get; set; }

            // Default Constructor
            [KittopiaConstructor(KittopiaConstructor.ParameterType.Empty)]
            public LandClassScatterLoader()
            {
                // Initialize default parameters
                Value = new PQSLandControl.LandClassScatter
                {
                    maxCache = 512, maxCacheDelta = 32, maxSpeed = 1000
                };

                // Get the Scatter-Parent
                GameObject scatterParent = new GameObject("Scatter");
                scatterParent.transform.parent = Utility.Deactivator;

                // Add the scatter module
                Scatter = scatterParent.AddOrGetComponent<ModularScatter>();
                Scatter.scatter = Value;

                // Create the Component callback
                Components = new CallbackList<ComponentLoader<ModularScatter>>(e =>
                {
                    Scatter.Components = Components.Select(c => c.Value).ToList();
                });
            }

            // Runtime constructor
            public LandClassScatterLoader(PQSLandControl.LandClassScatter value)
            {
                Value = value;

                // Get the Scatter-Parent
                GameObject scatterParent = typeof(PQSLandControl.LandClassScatter)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .FirstOrDefault(f => f.FieldType == typeof(GameObject))?.GetValue(Value) as GameObject;

                // If the GameObject is null, create one
                if (scatterParent == null)
                {
                    scatterParent = new GameObject("Scatter");
                    scatterParent.transform.parent = Utility.Deactivator;
                }

                // Add the scatter module
                Scatter = scatterParent.AddOrGetComponent<ModularScatter>();
                Scatter.scatter = Value;
                if (Value.baseMesh && Value.baseMesh.name != "Kopernicus-CubeDummy")
                {
                    Scatter.baseMesh = Value.baseMesh;
                }

                // Create the Component callback
                Components = new CallbackList<ComponentLoader<ModularScatter>>(e =>
                {
                    Scatter.Components = Components.Select(c => c.Value).ToList();
                });

                // Load existing Modules
                for (Int32 i = 0; i < Scatter.Components.Count; i++)
                {
                    Type componentType = Scatter.Components[i].GetType();
                    Type componentLoaderType =
                        typeof(ComponentLoader<,>).MakeGenericType(typeof(ModularScatter), componentType);

                    for (Int32 j = 0; j < Parser.ModTypes.Count; j++)
                    {
                        if (!componentLoaderType.IsAssignableFrom(Parser.ModTypes[j]))
                        {
                            continue;
                        }

                        // We found our loader type
                        ComponentLoader<ModularScatter> loader =
                            (ComponentLoader<ModularScatter>) Activator.CreateInstance(Parser.ModTypes[j]);
                        loader.Create(Scatter.Components[i]);
                        Components.Add(loader);
                    }
                }
            }

            /// <summary>
            /// Convert Parser to Value
            /// </summary>
            public static implicit operator PQSLandControl.LandClassScatter(LandClassScatterLoader parser)
            {
                return parser.Value;
            }

            /// <summary>
            /// Convert Value to Parser
            /// </summary>
            public static implicit operator LandClassScatterLoader(PQSLandControl.LandClassScatter value)
            {
                return new LandClassScatterLoader(value);
            }
        }
        
        */
        
        // Loader for the Amount of a Scatter on a body
        /*[RequireConfigType(ConfigType.Node)]
        public class LandClassScatterAmountLoader : IPatchable, ITypeParser<PQSLandControl.LandClassScatterAmount>
        {
            // The value we are editing
            public PQSLandControl.LandClassScatterAmount Value { get; set; }

            // Should we delete the ScatterAmount?
            [ParserTarget("delete")]
            [KittopiaHideOption]
            public NumericParser<Boolean> Delete = false;

            // density
            [ParserTarget("density")]
            public NumericParser<Double> Density
            {
                get { return Value.density; }
                set { Value.density = value; }
            }

            // The name of the scatter used
            [ParserTarget("scatterName")]
            public String name
            {
                get { return Value.scatterName; }
                set { Value.scatterName = value; }
            }

            // Default Constructor
            [KittopiaConstructor(KittopiaConstructor.ParameterType.Empty)]
            public LandClassScatterAmountLoader()
            {
                Value = new PQSLandControl.LandClassScatterAmount();
            }

            // Runtime Constructor
            public LandClassScatterAmountLoader(PQSLandControl.LandClassScatterAmount amount)
            {
                Value = amount;
            }

            /// <summary>
            /// Convert Parser to Value
            /// </summary>
            public static implicit operator PQSLandControl.LandClassScatterAmount(LandClassScatterAmountLoader parser)
            {
                return parser.Value;
            }

            /// <summary>
            /// Convert Value to Parser
            /// </summary>
            public static implicit operator LandClassScatterAmountLoader(PQSLandControl.LandClassScatterAmount value)
            {
                return new LandClassScatterAmountLoader(value);
            }
        }*/
        
        
        [RequireConfigType(ConfigType.Node)]
        public class ScatterClassLoader : IPatchable, ITypeParser<PQSMod_BiomeScatter.ScatterClass>
        {
            // The value we are editing
            public PQSMod_BiomeScatter.ScatterClass Value { get; set; }

            // Should we delete the LandClass?
            [ParserTarget("delete")]
            [KittopiaHideOption]
            [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
            public NumericParser<Boolean> Delete = false;

            // The name of the landclass
            [ParserTarget("name")]
            public String name
            {
                get { return Value.className; }
                set { Value.className = value; }
            }

            [ParserTarget("biome")]
            public String biome
            {
                get { return Value.biomeName; }
                set { Value.biomeName = value; }
            }

            // List of scatters used
            [ParserTargetCollection("Scatters", AllowMerge = true)]
            [ParserTargetCollection("scatters", AllowMerge = true)]
            public CallbackList<LandControl.LandClassScatterAmountLoader> Scatter { get; set; }

            // Default constructor
            [KittopiaConstructor(KittopiaConstructor.ParameterType.Empty)]
            public ScatterClassLoader()
            {
                Value = new PQSMod_BiomeScatter.ScatterClass
                {
                    className = "Base",
                    biomeName = "None"
                };

                // Create the scatter list
                Scatter = new CallbackList<LandControl.LandClassScatterAmountLoader>(e =>
                {
                    Value.scatters = Scatter.Where(s => !s.Delete).Select(s => s.Value).ToArray();
                });
                Value.scatters = new PQSLandControl.LandClassScatterAmount[0];
            }

            // Runtime constructor
            public ScatterClassLoader(PQSMod_BiomeScatter.ScatterClass value)
            {
                Value = value;

                // Create the scatter list
                Scatter = new CallbackList<LandControl.LandClassScatterAmountLoader>(e =>
                {
                    Value.scatters = Scatter.Where(s => !s.Delete).Select(s => s.Value).ToArray();
                });

                // Load scatters
                if (Value.scatters != null)
                {
                    for (Int32 i = 0; i < Value.scatters.Length; i++)
                    {
                        // Only activate the callback if we are adding the last loader
                        Scatter.Add(new LandControl.LandClassScatterAmountLoader(Value.scatters[i]),
                            i == Value.scatters.Length - 1);
                    }
                }
                else
                {
                    Value.scatters = new PQSLandControl.LandClassScatterAmount[0];
                }
            }

            /// <summary>
            /// Convert Parser to Value
            /// </summary>
            public static implicit operator PQSMod_BiomeScatter.ScatterClass(ScatterClassLoader parser)
            {
                return parser.Value;
            }

            /// <summary>
            /// Convert Value to Parser
            /// </summary>
            public static implicit operator ScatterClassLoader(PQSMod_BiomeScatter.ScatterClass value)
            {
                return new ScatterClassLoader(value);
            }
        }
        
        
        // List of scatters
        [ParserTargetCollection("Scatters", AllowMerge = true)]
        [ParserTargetCollection("scatters", AllowMerge = true)]
        public CallbackList<LandControl.LandClassScatterLoader> Scatters { get; set; }

        // List of landclasses
        [ParserTargetCollection("Classes", AllowMerge = true)]
        [ParserTargetCollection("classes", AllowMerge = true)]
        public CallbackList<ScatterClassLoader> ScatterClasses { get; set; }

        // Creates the a PQSMod of type T with given PQS
        public override void Create(PQS pqsVersion)
        {
            Debug.Log("[BIOME] 1");
            base.Create(pqsVersion);

            // Create the callback list for Scatters
            Scatters = new CallbackList<LandControl.LandClassScatterLoader>(e =>
            {
                foreach (LandControl.LandClassScatterLoader loader in Scatters)
                {
                    loader.Scatter.transform.parent = Mod.transform;
                }

                Mod.scatters = Scatters.Where(scatter => !scatter.Delete)
                    .Select(scatter => scatter.Value).ToArray();
            });
            Mod.scatters = new PQSLandControl.LandClassScatter[0];

            // Create the callback list for LandClasses
            ScatterClasses = new CallbackList<ScatterClassLoader>(e =>
            {
                Debug.Log("[BIOME] Number of amounts : " + e.Scatter.Count);
                // Assign each scatter amount with their corresponding scatter
                foreach (PQSLandControl.LandClassScatterAmount amount in e.Scatter)
                {
                    Int32 i = 0;
                    while (i < Mod.scatters.Length)
                    {
                        if (Mod.scatters[i].scatterName.Equals(amount.scatterName))
                        {
                            break;
                        }

                        i++;
                    }

                    if (i >= Mod.scatters.Length)
                    {
                        continue;
                    }
                    amount.scatterIndex = i;
                    amount.scatter = Mod.scatters[i];
                }

                // Assign the new values
                Mod.scatterClasses = ScatterClasses.Where(landClass => !landClass.Delete)
                    .Select(landClass => landClass.Value).ToArray();
                Debug.Log("[BIOME] Length2 : " + Mod.scatterClasses.Length);
            });
            Mod.scatterClasses = new PQSMod_BiomeScatter.ScatterClass[0];
            Debug.Log("[BIOME] Length : " + Mod.scatterClasses.Length);
        }

        // Grabs a PQSMod of type T from a parameter with a given PQS
        public override void Create(PQSMod_BiomeScatter mod, PQS pqsVersion)
        {
            Debug.Log("[BIOME] 2");
            base.Create(mod, pqsVersion);

            // Create the callback list for Scatters
            Scatters = new CallbackList<LandControl.LandClassScatterLoader>(e =>
            {
                foreach (LandControl.LandClassScatterLoader loader in Scatters)
                {
                    loader.Scatter.transform.parent = Mod.transform;
                }

                Mod.scatters = Scatters.Where(scatter => !scatter.Delete)
                    .Select(scatter => scatter.Value).ToArray();
            });

            // Load Scatters
            if (Mod.scatters != null)
            {
                for (Int32 i = 0; i < Mod.scatters.Length; i++)
                {
                    // Only activate the callback if we are adding the last loader
                    Scatters.Add(new LandControl.LandClassScatterLoader(Mod.scatters[i]), i == Mod.scatters.Length - 1);
                }
            }
            else
            {
                Mod.scatters = new PQSLandControl.LandClassScatter[0];
            }

            // Create the callback list for LandClasses
            ScatterClasses = new CallbackList<ScatterClassLoader>(e =>
            {
                // Assign each scatter amount with their corresponding scatter
                foreach (PQSLandControl.LandClassScatterAmount amount in e.Scatter)
                {
                    Int32 i = 0;
                    while (i < Mod.scatters.Length)
                    {
                        if (Mod.scatters[i].scatterName.Equals(amount.scatterName))
                        {
                            break;
                        }

                        i++;
                    }

                    if (i >= Mod.scatters.Length)
                    {
                        continue;
                    }
                    amount.scatterIndex = i;
                    amount.scatter = Mod.scatters[i];
                }

                // Assign the new values
                Mod.scatterClasses = ScatterClasses.Where(landClass => !landClass.Delete)
                    .Select(landClass => landClass.Value).ToArray();
            });

            // Load LandClasses
            if (Mod.scatterClasses != null)
            {
                for (Int32 i = 0; i < Mod.scatterClasses.Length; i++)
                {
                    // Only activate the callback if we are adding the last loader
                    ScatterClasses.Add(new ScatterClassLoader(Mod.scatterClasses[i]), i == Mod.scatterClasses.Length - 1);
                }
            }
            else
            {
                Mod.scatterClasses = new PQSMod_BiomeScatter.ScatterClass[0];
            }
        }
    }
}