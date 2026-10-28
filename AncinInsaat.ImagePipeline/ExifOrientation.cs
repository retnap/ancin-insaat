using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace AncinInsaat.ImagePipeline;

// Applies only the rotation part of the EXIF Orientation tag (2026-10-28),
// called by both generators before they strip EXIF. Camera originals shot in
// portrait are often stored landscape with Orientation 6/8, and once EXIF is
// gone nothing tells the browser to rotate them.
//
// Mirror flags are deliberately ignored rather than applied (AutoOrient would
// apply them): some originals (e.g. Ferhunde Hanım A Tipi renders) carry
// Orientation 2/4 as a stray tag on images that are already correct, and
// honouring it would mirror them on the next "ThumbnailTool --force".
//   3     -> rotate 180
//   6     -> rotate 90 (clockwise)
//   8     -> rotate 270 (clockwise)
//   5     -> rotate 270, mirror ignored ("mirror horizontal, rotate 270 CW")
//   7     -> rotate 90, mirror ignored  ("mirror horizontal, rotate 90 CW")
//   1/2/4 -> unchanged
internal static class ExifOrientation
{
    public static void ApplyRotationOnly(Image image)
    {
        if (image.Metadata.ExifProfile is null
            || !image.Metadata.ExifProfile.TryGetValue(ExifTag.Orientation, out var orientation))
        {
            return;
        }

        var rotateMode = orientation.Value switch
        {
            3 => RotateMode.Rotate180,
            6 or 7 => RotateMode.Rotate90,
            8 or 5 => RotateMode.Rotate270,
            _ => RotateMode.None
        };

        if (rotateMode != RotateMode.None)
        {
            image.Mutate(ctx => ctx.Rotate(rotateMode));
        }
    }
}
