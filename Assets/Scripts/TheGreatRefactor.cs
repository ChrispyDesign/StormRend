using UnityEngine;

namespace StormRend
{
    namespace The.Great.Refactor
    {
        public class Architecture
        {/*

            
            
            
            
            
            
        */}

        public class Renames
        {
            


        }

        public class UnitTesting
        {

        }

        public class Conventions
        {
            //Fields/Symbols
            [SerializeField] float privateShownOnInspector;
            [HideInInspector] [SerializeField] float PrivateNotShownOnInspectorButSerialized;
            public float avoidMe;     //Free variable that can be modified by anything and anyone
            
            //Properties
            //Shown on inspector, but read only in the assembly/codebase
            [SerializeField] float _propertyBackingField;
            public float propertyBackingField {
                get => _propertyBackingField;
                
            }

            void Something()
            {
                Debug.Log("somethign");
            }

            void UseExpressionBodyMethodsForCleanerCode() => Debug.Log("This is clean!");


            //Privates
            bool isPrivate = true;      //Implicit private



            
            
            
            /*


            Big classes
            - Classes over 200-300 lines of code should be split up using partial

            Try catch blocks
            try
            {
                if (blah)
                else if (bleh)
                else
                    throw new InvalidOperationException("This is illegal!");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        */}
    }
}