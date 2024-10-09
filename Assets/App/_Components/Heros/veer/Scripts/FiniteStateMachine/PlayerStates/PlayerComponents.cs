using UnityEngine;
using UnityEngine.U2D.IK;

[System.Serializable]
public class PlayerComponents 
{
    [SerializeField]
    public Body Body;

    [SerializeField]
    public Fist Fist;

    [SerializeField]
    public Sword Sword;

    [SerializeField]
    public Pistol Pistol;

    [SerializeField]
    public Rifle Rifle;

    [SerializeField]
    public Bow Bow;

    [SerializeField]
    public Gloves Gloves;

    public PlayerComponents()
    {
        Body = new Body();
        Fist = new Fist();
        Sword = new Sword();
        Pistol = new Pistol();
        Rifle = new Rifle();
        Bow = new Bow();
        Gloves = new Gloves();
    }
}

[System.Serializable]
public class Body
{
    [SerializeField]
    public Transform Head;

    public Solver2D LeftArmIKSolver;
    public Solver2D LeftFistIKSolver;

    public LimbSolver2D RightArmIKSolver;
    public Solver2D RightFistIKSolver;
    [Space(10)]

    [Header("Animated Targets")]
    [SerializeField]
    public Transform AnimLeftArmTarget;
    [SerializeField]
    public Transform AnimLeftFistTarget;
    [SerializeField]
    public Transform AnimRightArmTarget;
    [SerializeField]
    public Transform AnimRightFistTarget;
}

[System.Serializable]
public class Bow
{
    [SerializeField]
    public GameObject BowGameObj;

    public CCDSolver2D BowTopCCDIK;
    public CCDSolver2D BowBottomCCDIK;

    public Transform P_Bow_Parent_LeftHandTarget;
    [SerializeField]
    public Transform P_Bow_Parent_RightHandTarget;


    

    [SerializeField]
    public Transform P_BOW_LeftArmTarget;
    [SerializeField]
    public Transform P_BOW_LeftFistTarget;

    [SerializeField]
    public Transform P_BOW_RightArmTarget;
    [SerializeField]
    public Transform P_BOW_RightFistTarget;

    [SerializeField]
    public Transform BowFistTarget;




    [SerializeField]
    public Transform BowInitialTarget;

    [Space(10)]


    [Header("Arrow")]

    [SerializeField]
    public Transform ArrowHolder;

    [SerializeField]
    public GameObject ArrowPrefab;

    public GameObject CurrentArrowObject;

    [SerializeField]
    public GameObject _arrowPostion;

    public bool IsArrowAvailable = false;

}

[System.Serializable]
public class Rifle
{
    [SerializeField]
    public GameObject AkmGameObj;
    [SerializeField]
    public Transform P_AKM_Parent_LeftHandTarget;
    [SerializeField]
    public Transform P_AKM_Parent_RightHandTarget;

    [SerializeField]
    public Transform P_AKM_LeftArmTarget;
    [SerializeField]
    public Transform P_AKM_LeftFistTarget;

    public Transform P_AKM_RightArmTarget;
    [SerializeField]
    public Transform P_AKM_RightFistTarget;
}

[System.Serializable]
public class Pistol 
{
    [SerializeField]
    public GameObject PistolGameObj;

    [SerializeField]
    public Transform P_Revolver_Parent_LeftHandTarget;
    [SerializeField]
    public Transform P_Revolver_Parent_RightHandTarget;

    public Transform P_Revolver_LeftArmTarget;
    [SerializeField]
    public Transform P_Revolver_LeftFistTarget;

    [SerializeField]
    public Transform P_Revolver_RightArmTarget;
    [SerializeField]
    public Transform P_Revolver_RightFistTarget;
}

[System.Serializable]
public class Fist
{
    [SerializeField] public float a;
}

[System.Serializable]
public class Sword
{
    [SerializeField]
    public GameObject SwordGameObj;
    [SerializeField] public float a;
}

[System.Serializable]
public class Gloves
{
    [SerializeField]
    public GameObject GlovesGameObj;

    [SerializeField]
    public GameObject LeftGlovesGameObj;
}