using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Graphics;
using Android.Content.Res;
using Android.Media;
using Java.IO;
using Java.Nio;
using Java.Lang;

namespace FragmentsTestApp
{
    public class VideoFragment : Fragment
    {
        private static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();

        private Size mPreviewSize;
        private bool mOpeningCamera;
        private CameraDevice mCameraDevice;
        private AutoFitTextureView mTextureView;
        private CaptureRequest.Builder mPreviewBuilder;
        private CameraCaptureSession mPreviewSession;

        private Camera2BasicSurfaceTextureListener mSurfaceTextureListener;
        private class Camera2BasicSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
        {
            private VideoFragment Fragment;
            public Camera2BasicSurfaceTextureListener(VideoFragment fragment)
            {
                Fragment = fragment;
            }
            public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int width, int height)
            {
                Fragment.ConfigureTransform(width, height);
            }
            public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
            {
                return true;
            }
            public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
            {
                Fragment.ConfigureTransform(width, height);
                Fragment.StartPreview();
            }
            public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
            {

            }
        }

        CameraStateListener mStateListener;
        private class CameraStateListener : CameraDevice.StateCallback
        {
            public VideoFragment Fragment;
            public override void OnOpened(CameraDevice camera)
            {
                if(Fragment != null)
                {
                    Fragment.mCameraDevice = camera;
                    Fragment.StartPreview();
                    Fragment.mOpeningCamera = false;
                }
            }
            public override void OnDisconnected(CameraDevice camera)
            {
                if(Fragment != null)
                {
                    camera.Close();
                    Fragment.mCameraDevice = null;
                    Fragment.mOpeningCamera = false;
                }
            }
            public override void OnError(CameraDevice camera, [GeneratedEnum] Android.Hardware.Camera2.CameraError error)
            {
                camera.Close();
                if(Fragment != null)
                {
                    Fragment.mCameraDevice = null;
                    Activity activity = Fragment.Activity;
                    Fragment.mOpeningCamera = false;
                    if(activity != null)
                    {
                        activity.Finish();
                    }
                }
            }
        }

        private class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
        {
            public File File;
            public void OnImageAvailable(ImageReader reader)
            {
                Image image = null;
                try
                {
                    image = reader.AcquireLatestImage();
                    ByteBuffer buffer = image.GetPlanes()[0].Buffer;
                    byte[] bytes = new byte[buffer.Capacity()];
                    buffer.Get(bytes);
                    Save(bytes);
                }
                catch(FileNotFoundException ex)
                {
                    Log.WriteLine(LogPriority.Info, "Camera capture session", ex.StackTrace);
                }
                catch(IOException ex)
                {
                    Log.WriteLine(LogPriority.Info, "Camera capture session", ex.StackTrace);
                }
                finally
                {
                    if (image != null)
                        image.Close();
                }
            }

            private void Save(byte[] bytes)
            {
                OutputStream output = null;
                try
                {
                    if(File != null)
                    {
                        output = new FileOutputStream(File);
                        output.Write(bytes);
                    }
                }
                finally
                {
                    if (output != null)
                        output.Close();
                }
            }
        }

        private class CameraCaptureListener : CameraCaptureSession.CaptureCallback
        {
            public VideoFragment Fragment;
            public File File;
            public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
            {
                if(Fragment != null && File != null)
                {
                    Activity activity = Fragment.Activity;
                    if(activity != null)
                    {
                        Toast.MakeText(activity, "Saved: " + File.ToString(), ToastLength.Short).Show();
                        Fragment.StartPreview();
                    }
                }
            }
        }

        private class CameraCaptureStateListener : CameraCaptureSession.StateCallback
        {
            public Action<CameraCaptureSession> OnConfigureFailedAction;
            public override void OnConfigureFailed (CameraCaptureSession session)
            {
                if(OnConfigureFailedAction != null)
                {
                    OnConfigureFailedAction(session);
                }
            }

            public Action<CameraCaptureSession> OnConfiguredAction;
            public override void OnConfigured(CameraCaptureSession session)
            {
                if(OnConfiguredAction != null)
                {
                    OnConfiguredAction(session);
                }
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mStateListener = new CameraStateListener() { Fragment = this };
            mSurfaceTextureListener = new Camera2BasicSurfaceTextureListener(this);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 90);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 0);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 270);
            ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 180);
        }

        public static VideoFragment NewInstance()
        {
            VideoFragment fragment = new VideoFragment();
            fragment.RetainInstance = true;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
        }

        private void StartPreview()
        {
            throw new NotImplementedException();
        }

        private void ConfigureTransform(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}