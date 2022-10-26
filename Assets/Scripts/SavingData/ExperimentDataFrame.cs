using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class ExperimentDataFrame 
{
    public string ParticipantID;
    public List<string> TaskOrder;
    public string Language;

    public Vector3 TheNorth;
    public Vector3 InitialPointPosition;
    public Quaternion InitialPointRotation;
    public Vector3 InitialPointDirection;
        
    public List<int> Block_1_PointingNorthLocations;
    public List<float> Block_1_PointingNorthOrientation;
    
    public List<List<int>> Block_1_PointingBuildingLocations;   // not serializable with the used saving function,
                                                                // therefore the values are added to the 2 following variables

    public List<int> Block_1_PointingBuildingStartingLocations;
    public List<int> Block_1_PointingBuildingTargetLocations;

    public List<int> Block_2_PointingNorthLocations;
    public List<float> Block_2_PointingNorthOrientation;
    
    public List<List<int>> Block_2_PointingBuildingLocations;   // not serializable with the used saving function,
                                                                // therefore the values are added to the 2 following variables
    public List<int> Block_2_PointingBuildingStartingLocations;
    public List<int> Block_2_PointingBuildingTargetLocations;
    
    public List<int> GhostHouseStartingPositions;
    public List<List<int>> GhostHouseTargetLocations;   // not serializable with the used saving function,
                                                        // therefore the values are added to the 7 following variables
    
    // these are 7 because in this experiment we have 7 locations. By changing the number of
    // locations, these need to be adjusted as well
    public List<int> GhostHouseTargetLocations_1;
    public List<int> GhostHouseTargetLocations_2;
    public List<int> GhostHouseTargetLocations_3;
    public List<int> GhostHouseTargetLocations_4;
    public List<int> GhostHouseTargetLocations_5;
    public List<int> GhostHouseTargetLocations_6;
    public List<int> GhostHouseTargetLocations_7;
}
