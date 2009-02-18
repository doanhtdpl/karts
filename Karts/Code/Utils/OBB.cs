using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Karts.Code
{
    public struct OBB : IEquatable<OBB>
    {
        #region Fields
        /// <summary>
        /// Gets or sets the center of the bounding box
        /// </summary>
        private Vector3 Center;

        private Vector3 m_vBounds;
        private BoundingBox m_AxisAlignedLocalBoundingBox;
        private Matrix m_matRotation;
        private Matrix m_matInverseRotation;

#if DEBUG
        static VertexPositionColor[] verts = new VertexPositionColor[8];
        static int[] indices = new int[] { 0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4 };

        static BasicEffect effect;
        static VertexDeclaration vertDecl;
#endif
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bounds represents the half of the box width, height and length
        /// </summary>
        public Vector3 Bounds
        {
            get { return m_vBounds; }
            set
            {
                m_vBounds = value;
                m_AxisAlignedLocalBoundingBox = new BoundingBox(-m_vBounds, m_vBounds);
            }
        }

        /// <summary>
        /// Gets the local bounding box where min = -Bounds and max = Bounds
        /// </summary>
        public BoundingBox LocalBoundingBox
        {
            get { return m_AxisAlignedLocalBoundingBox; }
        }

        /// <summary>
        /// Gets or sets the rotation matrix of the obb (the inverse will be auto updated)
        /// the matrix must be only rotation matrix or must function won't work
        /// </summary>
        public Matrix RotationMatrix
        {
            get { return m_matRotation; }
            set
            {
                m_matRotation = value;
                m_matInverseRotation = Matrix.Transpose(m_matRotation);
            }
        }

        /// <summary>
        /// Gets or sets the inverse matrix of the rotation matrix (the rotation will be auto updated
        /// </summary>
        public Matrix InverseRotationMatrix
        {
            get { return m_matInverseRotation; }
            set
            {
                m_matInverseRotation = value;
                m_matRotation = Matrix.Transpose(m_matInverseRotation);
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates new bounding obb
        /// </summary>
        /// <param name="vCenter">the center of the obb</param>
        /// <param name="vBounds">vector that describes the half of width,height,length</param>
        /// <param name="matRotation">the rotation matrix of the obb</param>
        public OBB(Vector3 vCenter, Vector3 vBounds, Matrix matRotation)
        {
            Center = vCenter;
            m_vBounds = vBounds;
            m_matRotation = matRotation;
            m_matInverseRotation = Matrix.Transpose(m_matRotation);
            m_AxisAlignedLocalBoundingBox = new BoundingBox(-m_vBounds, m_vBounds);
        }

        /// <summary>
        /// Creates new bounding obb with rotation matrix as identity
        /// </summary>
        /// <param name="vCenter">the center of the obb</param>
        /// <param name="vBounds">vector that describes the half of width,height,length</param>
        public OBB(Vector3 vCenter, Vector3 vBounds)
        {
            Center = vCenter;
            m_vBounds = vBounds;
            m_matRotation = Matrix.Identity;
            m_matInverseRotation = Matrix.Identity;
            m_AxisAlignedLocalBoundingBox = new BoundingBox(-m_vBounds, m_vBounds);
        }
        #endregion

        #region Functions
        /// <summary>
        /// returns all 8 corners of the cube
        /// </summary>
        /// <param name="Corners">the array which the output will be assigned to</param>
        public void GetCorners(Vector3[] Corners)
        {
            m_AxisAlignedLocalBoundingBox.GetCorners(Corners);

            for (int i = 0; i < 8; i++)
            {
                Corners[i] = Vector3.Transform(Corners[i], m_matRotation) + Center;
            }
        }

        /// <summary>
        /// returns the 8 corners of the cube
        /// </summary>
        /// <returns>array containig the corners</returns>
        public Vector3[] GetCorners()
        {
            Vector3[] vCorners = new Vector3[8];
            GetCorners(vCorners);
            return vCorners;
        }

        /// <summary>
        /// check for instersection of sphere with this obb
        /// return true if there is a collision
        /// </summary>
        /// <param name="sphere">the sphere you whant to check collison for</param>
        /// <returns>true if collide</returns>
        public bool Intersects(BoundingSphere sphere) // OK
        {
            return m_AxisAlignedLocalBoundingBox.Intersects(new BoundingSphere(Vector3.Transform(sphere.Center - Center, m_matInverseRotation), sphere.Radius));
        }

        /// <summary>
        /// check for instersection of sphere with this obb
        /// </summary>
        /// <param name="sphere">the sphere you whant to check collison for</param>
        /// <param name="result">the place where the results will be assigned true if collide</param>
        public void Intersects(ref BoundingSphere sphere, out bool result) // OK
        {
            result = Intersects(sphere);
        }

        /// <summary>
        /// checks for instersection of an obb with an obb
        /// </summary>
        /// <param name="obb">the obb to do the test with</param>
        /// <returns>true if there is collosion</returns>
        public bool Intersects(OBB obb) // OK
        {
            Matrix matB = obb.RotationMatrix * this.InverseRotationMatrix;
            Vector3 vPosB = Vector3.Transform(obb.Center - this.Center,
                                              this.InverseRotationMatrix);

            Vector3 XAxis = new Vector3(matB.M11, matB.M21, matB.M31);
            Vector3 YAxis = new Vector3(matB.M12, matB.M22, matB.M32);
            Vector3 ZAxis = new Vector3(matB.M13, matB.M23, matB.M33);

            //15 tests

            //1 (Ra)x
            if ((float)Math.Abs(vPosB.X) >
                    (this.Bounds.X +
                        obb.Bounds.X * (float)Math.Abs(XAxis.X) +
                        obb.Bounds.Y * (float)Math.Abs(XAxis.Y) +
                        obb.Bounds.Z * (float)Math.Abs(XAxis.Z)))
            {
                return false;
            }

            //2 (Ra)y
            if ((float)Math.Abs(vPosB.Y) >
                    (this.Bounds.Y +
                        obb.Bounds.X * (float)Math.Abs(YAxis.X) +
                        obb.Bounds.Y * (float)Math.Abs(YAxis.Y) +
                        obb.Bounds.Z * (float)Math.Abs(YAxis.Z)))
            {
                return false;
            }

            //3 (Ra)z
            if ((float)Math.Abs(vPosB.Z) >
                    (this.Bounds.Z +
                        obb.Bounds.X * (float)Math.Abs(ZAxis.X) +
                        obb.Bounds.Y * (float)Math.Abs(ZAxis.Y) +
                        obb.Bounds.Z * (float)Math.Abs(ZAxis.Z)))
            {
                return false;
            }

            //4 (Rb)x
            if ((float)Math.Abs(vPosB.X * XAxis.X +
                                vPosB.Y * YAxis.X +
                                vPosB.Z * ZAxis.X) >
                    (obb.Bounds.X +
                        this.Bounds.X * (float)Math.Abs(XAxis.X) +
                        this.Bounds.Y * (float)Math.Abs(YAxis.X) +
                        this.Bounds.Z * (float)Math.Abs(ZAxis.X)))
            {
                return false;
            }

            //5 (Rb)y
            if ((float)Math.Abs(vPosB.X * XAxis.Y +
                                vPosB.Y * YAxis.Y +
                                vPosB.Z * ZAxis.Y) >
                    (obb.Bounds.Y +
                        this.Bounds.X * (float)Math.Abs(XAxis.Y) +
                        this.Bounds.Y * (float)Math.Abs(YAxis.Y) +
                        this.Bounds.Z * (float)Math.Abs(ZAxis.Y)))
            {
                return false;
            }

            //6 (Rb)z
            if ((float)Math.Abs(vPosB.X * XAxis.Z +
                                vPosB.Y * YAxis.Z +
                                vPosB.Z * ZAxis.Z) >
                    (obb.Bounds.Z +
                        this.Bounds.X * (float)Math.Abs(XAxis.Z) +
                        this.Bounds.Y * (float)Math.Abs(YAxis.Z) +
                        this.Bounds.Z * (float)Math.Abs(ZAxis.Z)))
            {
                return false;
            }

            //7 (Ra)x X (Rb)x
            if ((float)Math.Abs(vPosB.Z * YAxis.X -
                                vPosB.Y * ZAxis.X) >
                    (this.Bounds.Y * (float)Math.Abs(ZAxis.X) +
                    this.Bounds.Z * (float)Math.Abs(YAxis.X) +
                    obb.Bounds.Y * (float)Math.Abs(XAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(XAxis.Y)))
            {
                return false;
            }

            //8 (Ra)x X (Rb)y
            if ((float)Math.Abs(vPosB.Z * YAxis.Y -
                                vPosB.Y * ZAxis.Y) >
                    (this.Bounds.Y * (float)Math.Abs(ZAxis.Y) +
                    this.Bounds.Z * (float)Math.Abs(YAxis.Y) +
                    obb.Bounds.X * (float)Math.Abs(XAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(XAxis.X)))
            {
                return false;
            }

            //9 (Ra)x X (Rb)z
            if ((float)Math.Abs(vPosB.Z * YAxis.Z -
                                vPosB.Y * ZAxis.Z) >
                    (this.Bounds.Y * (float)Math.Abs(ZAxis.Z) +
                    this.Bounds.Z * (float)Math.Abs(YAxis.Z) +
                    obb.Bounds.X * (float)Math.Abs(XAxis.Y) +
                    obb.Bounds.Y * (float)Math.Abs(XAxis.X)))
            {
                return false;
            }

            //10 (Ra)y X (Rb)x
            if ((float)Math.Abs(vPosB.X * ZAxis.X -
                                vPosB.Z * XAxis.X) >
                    (this.Bounds.X * (float)Math.Abs(ZAxis.X) +
                    this.Bounds.Z * (float)Math.Abs(XAxis.X) +
                    obb.Bounds.Y * (float)Math.Abs(YAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(YAxis.Y)))
            {
                return false;
            }

            //11 (Ra)y X (Rb)y
            if ((float)Math.Abs(vPosB.X * ZAxis.Y -
                                vPosB.Z * XAxis.Y) >
                    (this.Bounds.X * (float)Math.Abs(ZAxis.Y) +
                    this.Bounds.Z * (float)Math.Abs(XAxis.Y) +
                    obb.Bounds.X * (float)Math.Abs(YAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(YAxis.X)))
            {
                return false;
            }

            //12 (Ra)y X (Rb)z
            if ((float)Math.Abs(vPosB.X * ZAxis.Z -
                                vPosB.Z * XAxis.Z) >
                    (this.Bounds.X * (float)Math.Abs(ZAxis.Z) +
                    this.Bounds.Z * (float)Math.Abs(XAxis.Z) +
                    obb.Bounds.X * (float)Math.Abs(YAxis.Y) +
                    obb.Bounds.Y * (float)Math.Abs(YAxis.X)))
            {
                return false;
            }

            //13 (Ra)z X (Rb)x
            if ((float)Math.Abs(vPosB.Y * XAxis.X -
                                vPosB.X * YAxis.X) >
                    (this.Bounds.X * (float)Math.Abs(YAxis.X) +
                    this.Bounds.Y * (float)Math.Abs(XAxis.X) +
                    obb.Bounds.Y * (float)Math.Abs(ZAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(ZAxis.Y)))
            {
                return false;
            }

            //14 (Ra)z X (Rb)y
            if ((float)Math.Abs(vPosB.Y * XAxis.Y -
                                vPosB.X * YAxis.Y) >
                    (this.Bounds.X * (float)Math.Abs(YAxis.Y) +
                    this.Bounds.Y * (float)Math.Abs(XAxis.Y) +
                    obb.Bounds.X * (float)Math.Abs(ZAxis.Z) +
                    obb.Bounds.Z * (float)Math.Abs(ZAxis.X)))
            {
                return false;
            }

            //15 (Ra)z X (Rb)z
            if ((float)Math.Abs(vPosB.Y * XAxis.Z -
                                vPosB.X * YAxis.Z) >
                    (this.Bounds.X * (float)Math.Abs(YAxis.Z) +
                    this.Bounds.Y * (float)Math.Abs(XAxis.Z) +
                    obb.Bounds.X * (float)Math.Abs(ZAxis.Y) +
                    obb.Bounds.Y * (float)Math.Abs(ZAxis.X)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// checks for instersection of an obb with an obb
        /// </summary>
        /// <param name="obb">the obb to do the test with</param>
        /// <param name="results">the variable that the result will be assainged to</param>
        public void Intersects(ref OBB obb, out bool results) // OK
        {
            results = Intersects(obb);
        }

        /// <summary>
        /// checks for instersection of an obb with an AABB
        /// </summary>
        /// <param name="box">the AABB to do the test with</param>
        /// <returns>true if there is collosion</returns>
        public bool Intersects(BoundingBox box) // OK
        {
            Vector3 vCenter = (box.Max + box.Min) * 0.5f;
            return Intersects(new OBB(vCenter, box.Max - vCenter));
        }

        /// <summary>
        /// checks for instersection of an obb with an AABB
        /// </summary>
        /// <param name="box">the AABB to do the test with</param>
        /// <param name="results">the variable that the result will be assainged to</param>
        public void Intersects(ref BoundingBox box, out bool results) // OK
        {
            Vector3 vCenter = (box.Max + box.Min) * 0.5f;
            results = Intersects(new OBB(vCenter, box.Max - vCenter));
        }

        /// <summary>
        /// checks for intersection of a obb with a plane
        /// </summary>
        /// <param name="plane">the plane to check the intersection with</param>
        /// <returns>PlaneIntersectionType describes the result</returns>
        public PlaneIntersectionType Intersects(Plane plane) // OK
        {
            PlaneIntersectionType type;
            Intersects(ref plane, out type);
            return type;
        }

        /// <summary>
        /// checks for intersection of a obb with a plane
        /// </summary>
        /// <param name="plane">the plane to check the intersection with</param>
        /// <param name="result">assagin PlaneIntersectionType describes the result to this variable</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result) // OK
        {
            Vector3[] Corners = GetCorners();
            float dot;
            result = PlaneIntersectionType.Intersecting;
            for (int i = 0; i < 8; i++)
            {
                dot = plane.DotCoordinate(Corners[i]);
                if (dot == 0)
                {
                    result = PlaneIntersectionType.Intersecting;
                    return;
                }
                else if (dot < 0)
                {
                    if (result == PlaneIntersectionType.Front)
                    {
                        result = PlaneIntersectionType.Intersecting;
                        return;
                    }
                    result = PlaneIntersectionType.Back;
                }
                else
                {
                    if (result == PlaneIntersectionType.Back)
                    {
                        result = PlaneIntersectionType.Intersecting;
                        return;
                    }
                    result = PlaneIntersectionType.Front;
                }
            }
        }

        /// <summary>
        /// checks for the intersection of the ray with the box
        /// </summary>
        /// <param name="ray">ray to check the intersection with</param>
        /// <param name="result">assagin the length from the ray start position to the intersection point to this</param>
        public void Intersects(ref Ray ray, out float? result) // OK
        {

            Ray transformed = new Ray(Vector3.Transform(ray.Position - Center, InverseRotationMatrix), Vector3.Transform(ray.Direction, InverseRotationMatrix));

            m_AxisAlignedLocalBoundingBox.Intersects(ref transformed, out result);
        }

        /// <summary>
        /// checks for the intersection of the ray with the box
        /// </summary>
        /// <param name="ray">ray to check the intersection with</param>
        /// <returns>the length from the ray start position to the intersection point to</returns>
        public float? Intersects(Ray ray) // OK
        {
            float? result;
            Intersects(ref ray, out result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="result"></param>
        public void Contains(ref Vector3 vector, out ContainmentType result) // OK
        {
            Vector3 vectorT = Vector3.Transform(vector - Center, InverseRotationMatrix);
            m_AxisAlignedLocalBoundingBox.Contains(ref vectorT, out result);
        }

        public ContainmentType Contains(Vector3 vector) // OK
        {
            return m_AxisAlignedLocalBoundingBox.Contains(Vector3.Transform(vector - Center, InverseRotationMatrix));
        }

        public void Contains(ref BoundingSphere sphere, out ContainmentType result) // OK
        {
            BoundingSphere sphereTra = new BoundingSphere(Vector3.Transform(sphere.Center - Center, InverseRotationMatrix), sphere.Radius);
            m_AxisAlignedLocalBoundingBox.Contains(ref sphereTra, out result);
        }

        public ContainmentType Contains(BoundingSphere sphere) // OK
        {
            return m_AxisAlignedLocalBoundingBox.Contains(new BoundingSphere(Vector3.Transform(sphere.Center - Center, InverseRotationMatrix), sphere.Radius));
        }

        public ContainmentType Contains(BoundingFrustum frustum) // OK
        {
            Vector3[] furCorners = frustum.GetCorners();
            int containsCorners = 0;
            foreach (Vector3 corner in furCorners)
            {
                if (Contains(corner) != ContainmentType.Disjoint)
                    containsCorners++;
            }
            if (containsCorners >= 8)
                return ContainmentType.Contains;
            else if (containsCorners > 0)
                return ContainmentType.Intersects;

            GetCorners(furCorners);
            foreach (Vector3 corner in furCorners)
            {
                if (frustum.Contains(corner) != ContainmentType.Disjoint)
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Disjoint;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result) // OK
        {
            Vector3[] boxCorners = box.GetCorners();
            int containsCorners = 0;
            foreach (Vector3 corner in boxCorners)
            {
                if (Contains(corner) != ContainmentType.Disjoint)
                    containsCorners++;
            }
            if (containsCorners >= 8)
            {
                result = ContainmentType.Contains;
                return;
            }
            else if (containsCorners > 0)
            {
                result = ContainmentType.Intersects;
                return;
            }

            GetCorners(boxCorners);
            foreach (Vector3 corner in boxCorners)
            {
                if (box.Contains(corner) != ContainmentType.Disjoint)
                {
                    result = ContainmentType.Intersects;
                    return;
                }
            }

            result = ContainmentType.Disjoint;
        }

        public ContainmentType Contains(BoundingBox box) // OK
        {
            Vector3[] boxCorners = box.GetCorners();
            int containsCorners = 0;
            foreach (Vector3 corner in boxCorners)
            {
                if (Contains(corner) != ContainmentType.Disjoint)
                    containsCorners++;
            }
            if (containsCorners >= 8)
                return ContainmentType.Contains;
            else if (containsCorners > 0)
                return ContainmentType.Intersects;

            GetCorners(boxCorners);
            foreach (Vector3 corner in boxCorners)
            {
                if (box.Contains(corner) != ContainmentType.Disjoint)
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Disjoint;
        }

        public void Contains(ref OBB obb, out ContainmentType result) // OK
        {
            Vector3[] boxCorners = obb.GetCorners();
            int containsCorners = 0;
            foreach (Vector3 corner in boxCorners)
            {
                if (Contains(corner) != ContainmentType.Disjoint)
                    containsCorners++;
            }
            if (containsCorners >= 8)
            {
                result = ContainmentType.Contains;
                return;
            }
            else if (containsCorners > 0)
            {
                result = ContainmentType.Intersects;
                return;
            }

            GetCorners(boxCorners);
            foreach (Vector3 corner in boxCorners)
            {
                if (obb.Contains(corner) != ContainmentType.Disjoint)
                {
                    result = ContainmentType.Intersects;
                    return;
                }
            }

            result = ContainmentType.Disjoint;
        }

        public ContainmentType Contains(OBB obb) // OK
        {
            Vector3[] boxCorners = obb.GetCorners();
            int containsCorners = 0;
            foreach (Vector3 corner in boxCorners)
            {
                if (Contains(corner) != ContainmentType.Disjoint)
                    containsCorners++;
            }
            if (containsCorners >= 8)
                return ContainmentType.Contains;
            else if (containsCorners > 0)
                return ContainmentType.Intersects;

            GetCorners(boxCorners);
            foreach (Vector3 corner in boxCorners)
            {
                if (obb.Contains(corner) != ContainmentType.Disjoint)
                    return ContainmentType.Intersects;
            }

            return ContainmentType.Disjoint;
        }

        public void Draw(Color c)
        {
#if DEBUG
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();
            GraphicsDevice graphicsDevice = gdm.GraphicsDevice;

            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice, null);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
                vertDecl = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);
            }

            Vector3[] corners = LocalBoundingBox.GetCorners();

            for (int i = 0; i < 8; i++)
            {
                verts[i].Position = corners[i] + Center;
                verts[i].Position = Vector3.Transform(verts[i].Position, m_matRotation);
                verts[i].Color = c;
            }

            graphicsDevice.VertexDeclaration = vertDecl;

            Camera cam = CameraManager.GetInstance().GetActiveCamera();

            effect.View = cam.GetViewMatrix();
            effect.Projection = cam.GetProjectionMatrix();

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, 8, indices, 0, indices.Length / 2);
                pass.End();
            }
            effect.End();
#endif
        }
        #endregion

        #region IEquatable<OBB> Members

        public bool Equals(OBB other)
        {
            return (other.m_vBounds == m_vBounds && other.Center == Center && other.m_matRotation == m_matRotation);
        }

        #endregion

        #region Object Overrides
        public override bool Equals(object obj)
        {
            if (obj is OBB)
                return Equals((OBB)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ Bounds.GetHashCode() ^ RotationMatrix.GetHashCode();
        }
        #endregion
    }
}
