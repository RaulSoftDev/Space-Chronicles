using UnityEngine;

namespace Mkey
{
    public class ParallaxPlane : MonoBehaviour
    {
        [SerializeField]
        private Transform mainContainer;
        private Transform additContainer; // along x
        private Transform additContainer_1; // along y
        private Transform additContainer_2; // along xy
        private Transform tempContainer;
        private float mapSizeX;
        private float mapSizeY;
        private float halfMapSizeX;
        private float halfMapSizeY;
        private Vector3 deltaPositionAC;  // delta position of additional container by x
        private Vector3 deltaPositionAC_1; // delta position of additional container by y
        private Vector3 deltaPositionAC_2; // delta position of additional container by xy
        private Vector3 mainContainerPosition;
        private bool canUpdate = false;

        /// <summary>
        /// Use this method for cretae 1d infinite map
        /// </summary>
        /// <param name="mapSizeX"></param>
        /// <param name="camera"></param>
        public void CreateInfinitePlane(float mapSizeX, float cameraPosX)
        {
            this.mapSizeX = mapSizeX;
            halfMapSizeX = mapSizeX / 2f;
            deltaPositionAC = new Vector3(mapSizeX, 0, 0);  // Debug.Log("Duplicate main container: " + name);
            if (mainContainer) additContainer = Instantiate(mainContainer, transform);
            canUpdate = (mainContainer && additContainer);
            UpdateInfinitePlane(cameraPosX);
        }

        /// <summary>
        /// Use this method for cretae 2d infinite map
        /// </summary>
        /// <param name="mapSizeX"></param>
        /// <param name="camera"></param>
        public void CreateInfinitePlane(Vector2 mapSize, Vector2 cameraPos)
        {
          //  Debug.Log("Create inf plane: " +name);
            this.mapSizeX = mapSize.x;
            this.mapSizeY = mapSize.y;
            halfMapSizeX = mapSizeX / 2f;
            halfMapSizeY = mapSizeY / 2f;
            deltaPositionAC = new Vector3(mapSizeX, 0, 0);  // Debug.Log("Duplicate main container: " + name);
            deltaPositionAC_1 = new Vector3( 0, mapSizeY, 0);  // Debug.Log("Duplicate main container: " + name);
            if (mainContainer)
            {
                additContainer = Instantiate(mainContainer, transform);
                additContainer_1 = Instantiate(mainContainer, transform);
                additContainer_2 = Instantiate(mainContainer, transform);
            }
            canUpdate = (mainContainer && additContainer && additContainer_1 && additContainer_2);
            UpdateInfinitePlane(cameraPos);
         
        }

        /// <summary>
        /// Use this method for update 1d infinite plane
        /// </summary>
        /// <param name="cameraPosX"></param>
        public void UpdateInfinitePlane(float cameraPosX)
        {
            if (!canUpdate) return;
            mainContainerPosition = mainContainer.position;
            //1 set addit container position
            if (cameraPosX > mainContainerPosition.x)
            {
                additContainer.position = mainContainerPosition + deltaPositionAC;
            }
            else
            {
                additContainer.position = mainContainerPosition - deltaPositionAC;
            }

            //2 swap containers mainContainer <-> additContainer
            if ((cameraPosX > mainContainerPosition.x + halfMapSizeX) || (cameraPosX < mainContainerPosition.x - halfMapSizeX))
            {
                tempContainer = mainContainer;
                mainContainer = additContainer;
                additContainer = tempContainer;
            }
        }

        /// <summary>
        /// Use this method for update 2d infinite plane
        /// </summary>
        /// <param name="cameraPosX"></param>
        public void UpdateInfinitePlane(Vector2 cameraPos)
        {
            if (!canUpdate) return;
            mainContainerPosition = mainContainer.position;
            Vector2 dPos = cameraPos - (Vector2)mainContainerPosition;

            //1 set addit container position along x
            if (dPos.x > 0) 
            {
                additContainer.position = mainContainerPosition + deltaPositionAC;
            }
            else
            {
                additContainer.position = mainContainerPosition - deltaPositionAC;
            }

            //2 set addit container position along y
            if (dPos.y > 0) 
            {
                additContainer_1.position = mainContainerPosition + deltaPositionAC_1;
            }
            else
            {
                additContainer_1.position = mainContainerPosition - deltaPositionAC_1;
            }

            additContainer_2.position = new Vector3(additContainer.position.x, additContainer_1.position.y, additContainer_1.position.z);


            //3 swap containers mainContainer <-> additContainer
            bool outx = false;
            bool outy = false;
            tempContainer = mainContainer;
            if ((cameraPos.x > mainContainerPosition.x + halfMapSizeX) || (cameraPos.x < mainContainerPosition.x - halfMapSizeX))
            {
                outx = true;
            }

            if ((cameraPos.y > mainContainerPosition.y + halfMapSizeY) || (cameraPos.y < mainContainerPosition.y - halfMapSizeY))
            {
                outy = true;
            }

            if(outx && !outy)
            {
                mainContainer = additContainer;
                additContainer = tempContainer;
            }
            else if(outx && outy)
            {
                mainContainer = additContainer_2;
                additContainer_2 = tempContainer;
            }
            else if(!outx && outy)
            {
                mainContainer = additContainer_1;
                additContainer_1 = tempContainer;
            }

           // mainContainer.name = "main"; additContainer.name = "add_0";  additContainer_1.name = "add_1"; additContainer_2.name = "add_2";

        }
    }
}