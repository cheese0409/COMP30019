# COMP30019-Project1

## Camera Control
- All controls were Inplemeted in Update() method, and using Unity Input API
- Mouse: Based on transform.Translate, get the horizontal and vertical axis value and multiply the predefined speed
- Keyboard: Based on transform.eulerAngles, get every frame's mouse position and multiply the the predefined speed

## Water
- Vertex Shader: Simulated the displacement using cosine function and apply speed, strengh and color variables to the vertex.
- Mesh: Created a plane instance and assigned it to water MeshFilter
- Position: Set the water mesh render's Y position equal to half of the terrain's height
- Phong illumination: Applied ambient, diffuse and specular reflections on the water so there is a reflection of the sun.

## Terrain
- DiamondSquare: Starting from initializing heights for four corners and for each diamond and square steps, we calculated the average height from four points and added a random noise in order to make the terrain realistic.
- Color: In order to properly display the rocky or snowy view of the terrain, we used a if-else if conditional statement to assign different colors for each part depending on their heights such as snow, rock, woods, beach and riverbed.
- Phong illumination: Applied ambient and diffuse reflection on the hill in mountain's shader.

## Sun
- Rotation: Used Lab 2 Code XAxisSpin.cs to implement the rotation
- Mesh: Apply a sphere MeshFilter and Sprites/Default shader

## Reference
- Water shader: Following the ideas from the video tutorial (https://www.youtube.com/watch?v=lWCPFwxZpVg)
- Lab 2 Code for Sun Rotation and Lab 5 Code for Phong illumination
- Parts of the diamond-square algorithm are retrieved from website (https://www.youtube.com/watch?v=1HV8GbFnCik), the method to create the landscape using the general idea of the source code from that link, and modified to suit this project.
- Most of the shaders, including the phong shader for the terrain and water shader, are retrieved from the COMP30019 workshop source code materials to complete this project.
