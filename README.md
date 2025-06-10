# Channel-packer-for-unity
A simple channel packer for easy texture conversion in unity for assets available on sketchfab and etc websites that are specifically not designed for HDRP or URP or Unreal ORM map

Download the latest release from the releases tab.

![Screenshot 2025-06-10 110152](https://github.com/user-attachments/assets/a2217d3b-4faa-4b5f-b896-0fedf17b2852)

as seen in the above image just drag and drop in the required textures in appropriate inputs. 

for example to convert Unity's old metallic smoothness map to new HDRP mask map
drop metallic smoothness map in the red channel and select sampling type to red channel
drop in AO map in green channel and select sampling type to red or green or anything in this case doesnt matter
typyically we leave blue to solid black unless we have a mask where we want a detailed mask if you do ahve it in any texture sample appropriate channel.
in alpha channel drop in metallic smoothness and select sampling type to alpha channel 
if you have roughness map then select sampling type to invert Red or invert alpha according to your texture type. 
