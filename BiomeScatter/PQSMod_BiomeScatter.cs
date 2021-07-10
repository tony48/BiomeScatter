using System;
using System.Collections.Generic;
using Kopernicus.Components;
using Kopernicus.Configuration;
using Kopernicus.Constants;
using Kopernicus.Configuration.Parsing;
using UnityEngine;


namespace TonyTest
{
    // ReSharper disable once InconsistentNaming
    public class PQSMod_BiomeScatter : PQSMod
    {
        public ScatterClass[] classes;
        public PQSLandControl.LandClassScatter[] scatters;
        private int scatterCount;
        private double[] scScatterList;
        private int scatterMinSubdiv;
        private bool scatterActive;
        private int scatterInstCount;
        
        /*private void Start()
        {
            GameEvents.OnGameSettingsApplied.Add(ApplyCastShadowsSetting);
        }

        private void OnDestroy()
        {
            GameEvents.OnGameSettingsApplied.Remove(ApplyCastShadowsSetting);
        }

        private void ApplyCastShadowsSetting()
        {
            if (scatters.Length == 0)
            {
                return;
            }
            for (int i = 0; i < scatters.Length; i++)
            {
                if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
                {
                    scatters[i].castShadows = true;
                    scatters[i].recieveShadows = true;
                }
                else
                {
                    scatters[i].castShadows = false;
                    scatters[i].recieveShadows = false;
                }
            }
        }*/
        
        public override void OnSetup()
        {
            requirements = PQS.ModiferRequirements.VertexMapCoords | PQS.ModiferRequirements.MeshColorChannel;
            Debug.LogWarning("[BIOME] Value of classes " + classes.Length.ToString());
            ScatterClass[] array = classes;
            
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].scatters.Length; j++)
                {
                    PQSLandControl.LandClassScatterAmount landClassScatterAmount = array[i].scatters[j];
                    for (int k = 0; k < scatters.Length; k++)
                    {
                        if (landClassScatterAmount.scatterName == scatters[k].scatterName)
                        {
                            landClassScatterAmount.scatter = scatters[k];
                            if (GameSettings.CELESTIAL_BODIES_CAST_SHADOWS)
                            {
                                landClassScatterAmount.scatter.castShadows = true;
                                landClassScatterAmount.scatter.recieveShadows = true;
                            }
                            else
                            {
                                landClassScatterAmount.scatter.castShadows = false;
                                landClassScatterAmount.scatter.recieveShadows = false;
                            }
                            landClassScatterAmount.scatterIndex = k;
                            break;
                        }
                    }
                }
            }

            if (PQS.Global_AllowScatter)
            {
                scatterCount = scatters.Length;
                scScatterList = new double[scatterCount];
                scatterMinSubdiv = int.MaxValue;
                for (int l = 0; l < scatters.Length; l++)
                {
                    PQSLandControl.LandClassScatter landClassScatter = scatters[l];
                    landClassScatter.Setup(sphere);
                    if (landClassScatter.minLevel < scatterMinSubdiv)
                    {
                        scatterMinSubdiv = landClassScatter.minLevel;
                    }
                }
            }
        }
        
        public override void OnQuadPreBuild(PQ quad)
        {
            if (quad.subdivision >= scatterMinSubdiv && PQS.Global_AllowScatter)
            {
                scatterActive = true;
            }
            else
            {
                scatterActive = false;
            }
        }

        public override void OnVertexBuild(PQS.VertexBuildData data)
        {
            for (int i = 0; i < classes.Length; i++)
            {
                ScatterClass sc = classes[i];
                if (data.allowScatter && scatterActive)
                {
                    PQSLandControl.LandClassScatterAmount[] scatter = sc.scatters;
                    for (int j = 0; j < scatter.Length; j++)
                    {
                        PQSLandControl.LandClassScatterAmount landClassScatterAmount = scatter[j];
                        if (data.buildQuad.subdivision >= scatterMinSubdiv)
                        {
                            scScatterList[landClassScatterAmount.scatterIndex] += landClassScatterAmount.density *
                                PQS.cacheVertCountReciprocal * PQS.Global_ScatterFactor;
                            scatterInstCount++;
                        }
                    }
                }
            }
        }
        
        public override void OnQuadBuilt(PQ quad)
        {
            if (!scatterActive || scatterInstCount <= 0)
            {
                return;
            }
            for (int i = 0; i < scatterCount; i++)
            {
                if (scScatterList[i] > 0.0)
                {
                    scatters[i].AddScatterMeshController(quad, scScatterList[i]);
                    scScatterList[i] = 0.0;
                }
            }
            scatterActive = false;
        }
        
        public override void OnSphereStarted()
        {
            for (int i = 0; i < scatters.Length; i++)
            {
                scatters[i].SphereActive();
            }
        }
        
        public override void OnSphereReset()
        {
            for (int i = 0; i < scatters.Length; i++)
            {
                scatters[i].SphereInactive();
            }
        }
    }
}