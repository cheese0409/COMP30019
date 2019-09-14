Description:
To create the terrain, we used diamond-square algorithm, starting from initializing heights for four corners and for each diamond and square steps, we calculated the average height from four points and added a random noise in order to make the terrain realistic. In addition, we set different colors to different altitudes of the mountain.Therefore, there is snow, rock, woods,beach and riverbed.   To make it realistic under the sunlights, we only applied ambient and diffuse reflection on the hill in mountain's shader.

In the water parts, we represented the water using a simple plane that go through the mountain. In the water shader, we applied wave effect and performed ambient, diffuse and specular reflections on the water. therefore, there is reflection of the sun in the water.



Resources: 
Parts of the diamond-square algorithm are retrieved from website https://www.youtube.com/watch?v=1HV8GbFnCik, the method to create the landscape using the general idea of the source code from that link, and modified to suit this project.

Most of the shaders, including the phong shader for the terrain and water shader, are retrieved from the COMP30019 workshop source code materials to complete this project.

