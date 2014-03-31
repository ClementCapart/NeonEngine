using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

namespace NeonEngine
{
	public class Water : Entity
	{
		struct WaterColumn
		{
			public float TargetHeight;
			public float Height;
			public float Speed;

			public void Update(float dampening, float tension)
			{
				float x = TargetHeight - Height;
				Speed += tension * x - Speed * dampening;
				Height += Speed;
			}
		}
		class WaterParticle
		{
			public Vector2 Position;
			public Vector2 Velocity;
			public float Orientation;

			public WaterParticle(Vector2 position, Vector2 velocity, float orientation)
			{
				Position = position;
				Velocity = velocity;
				Orientation = orientation;
			}
		}

		PrimitiveWater pb;
		WaterColumn[] columns;
        static Random rand = Neon.Utils.CommonRandom;
		Body collision;

		public float Tension = 0.08f;
		public float Dampening = 0.1f;
		public float Spread = 0.08f;
		public bool emitSplashParticles;
		public Color color = Color.YellowGreen;

		RenderTarget2D metaballTarget, particlesTarget;
		SpriteBatch spriteBatch;
		AlphaTestEffect alphaTest;
		Texture2D particleTexture;

		public Rectangle area;

		private float Scale { get { return area.Width / (columns.Length - 1f); } }
		public int Surface { get { return area.Y; } }

		List<WaterParticle> particles = new List<WaterParticle>();
		
		public Water(World containerWorld, Rectangle area)
			: base(containerWorld)
		{
			this.area = area;
			collision = BodyFactory.CreateRectangle(containerWorld.PhysicWorld, CoordinateConversion.screenToWorld(area.Width), CoordinateConversion.screenToWorld(area.Height), 1);
			collision.Position = new Vector2(CoordinateConversion.screenToWorld(area.X + area.Width / 2), CoordinateConversion.screenToWorld(area.Y + area.Height/2));
			collision.BodyType = BodyType.Kinematic;
			collision.IsSensor = true;
			collision.OnCollision += onCollision;
			collision.OnSeparation += onSeparation;
			emitSplashParticles = Neon.WaterSplash;
			columns = new WaterColumn[area.Width / 4];
			pb = new PrimitiveWater(containerWorld.game.GraphicsDevice);
			spriteBatch = new SpriteBatch(containerWorld.game.GraphicsDevice);
			particleTexture = Neon.Utils.generateRadialGradient(10);
			metaballTarget = new RenderTarget2D(Neon.GraphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);
			particlesTarget = new RenderTarget2D(Neon.GraphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);
			alphaTest = new AlphaTestEffect(Neon.GraphicsDevice);
			alphaTest.ReferenceAlpha = 175;

			var view = Neon.GraphicsDevice.Viewport;
			alphaTest.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
				Matrix.CreateOrthographicOffCenter(0, view.Width, view.Height, 0, 0, 1);

			for (int i = 0; i < columns.Length; i++)
			{
				columns[i] = new WaterColumn()
				{
					Height = area.Y,
					TargetHeight = area.Y,
					Speed = 0
				};
			}
		}

		// Returns the height of the water at a given x coordinate.
		public float GetHeight(float x)
		{
			int index = (int)((x - area.X) / Scale);
			return columns[index].Height;
		}

		void UpdateParticle(WaterParticle particle)
		{
			const float Gravity = 0.3f;
			particle.Velocity.Y += Gravity;
			particle.Position += particle.Velocity;
			particle.Orientation = GetAngle(particle.Velocity);
		}

		public void Splash(float xPosition, float speed)
		{
			int index = (int)MathHelper.Clamp((xPosition - area.X) / Scale, 0, columns.Length - 1);
			for (int i = Math.Max(0, index - 0); i < Math.Min(columns.Length - 1, index + 1); i++)
				columns[index].Speed = speed;

			if(emitSplashParticles)
				CreateSplashParticles(xPosition, speed);
		}

		private void CreateSplashParticles(float xPosition, float speed)
		{
			float y = GetHeight(xPosition);

			for (int i = 0; i < speed / 8; i++)
			{
				Vector2 pos = new Vector2(xPosition, y) + GetRandomVector2(40);
				Vector2 vel = FromPolar(MathHelper.ToRadians(GetRandomFloat(-150, -30)), GetRandomFloat(0, 0.5f * (float)Math.Sqrt(speed)));
				CreateParticle(pos, vel);
			}
		}

		#region Functions
		private void CreateParticle(Vector2 pos, Vector2 velocity)
		{
			particles.Add(new WaterParticle(pos, velocity, 0));
		}

		private Vector2 FromPolar(float angle, float magnitude)
		{
			return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		private float GetRandomFloat(float min, float max)
		{
			return (float)rand.NextDouble() * (max - min) + min;
		}

		private Vector2 GetRandomVector2(float maxLength)
		{
			return FromPolar(GetRandomFloat(-MathHelper.Pi, MathHelper.Pi), GetRandomFloat(0, maxLength));
		}

		private float GetAngle(Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}
		#endregion

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < columns.Length; i++)
				columns[i].Update(Dampening, Tension);

			float[] lDeltas = new float[columns.Length];
			float[] rDeltas = new float[columns.Length];

			// do some passes where columns pull on their neighbours
			for (int j = 0; j < 8; j++)
			{
				for (int i = 0; i < columns.Length; i++)
				{
					if (i > 0)
					{
						lDeltas[i] = Spread * (columns[i].Height - columns[i - 1].Height);
						columns[i - 1].Speed += lDeltas[i];
					}
					if (i < columns.Length - 1)
					{
						rDeltas[i] = Spread * (columns[i].Height - columns[i + 1].Height);
						columns[i + 1].Speed += rDeltas[i];
					}
				}

				for (int i = 0; i < columns.Length; i++)
				{
					if (i > 0)
						columns[i - 1].Height += lDeltas[i];
					if (i < columns.Length - 1)
						columns[i + 1].Height += rDeltas[i];
				}
			}

			if (emitSplashParticles)
			{
				foreach (var particle in particles)
					UpdateParticle(particle);
				particles = particles.Where(x => x.Position.X >= area.X && x.Position.X <= area.X + area.Width && x.Position.Y - 5 <= GetHeight(x.Position.X)).ToList();
			}

			DrawToRenderTargets();
		}

		public void DrawToRenderTargets()
		{
			if (emitSplashParticles)
			{
				GraphicsDevice device = spriteBatch.GraphicsDevice;
				device.SetRenderTarget(metaballTarget);
				device.Clear(Color.Transparent);

				// draw particles to the metaball render target
				spriteBatch.Begin(0, BlendState.Additive, null, null, null, null, GameWorld.Camera.get_transformation(Neon.GraphicsDevice));
				foreach (var particle in particles)
				{
					Vector2 origin = new Vector2(particleTexture.Width, particleTexture.Height) / 2f;
					spriteBatch.Draw(particleTexture, particle.Position, null, Color.White, particle.Orientation, origin, 2f, 0, 0);
				}
				spriteBatch.End();

				// draw a gradient above the water so the metaballs will fuse with the water's surface.
				pb.Begin(PrimitiveType.TriangleList);

				const float thickness = 10;
				float scale = Scale;
				for (int i = 1; i < columns.Length; i++)
				{
					Vector2 p1 = new Vector2((i - 1) * scale + area.X, columns[i - 1].Height + 2) - GameWorld.Camera.Position + Neon.HalfScreen;
					Vector2 p2 = new Vector2(i * scale + area.X, columns[i].Height + 2) - GameWorld.Camera.Position + Neon.HalfScreen;
					Vector2 p3 = new Vector2(p1.X, p1.Y - thickness);
					Vector2 p4 = new Vector2(p2.X, p2.Y - thickness);

					pb.AddVertex(p2, Color.White);
					pb.AddVertex(p1, Color.White);
					pb.AddVertex(p3, Color.Transparent);

					pb.AddVertex(p3, Color.Transparent);
					pb.AddVertex(p4, Color.Transparent);
					pb.AddVertex(p2, Color.White);
				}

				pb.End();

				device.SetRenderTarget(particlesTarget);
				device.Clear(Color.Transparent);
				spriteBatch.Begin(0, null, null, null, null, alphaTest);
				spriteBatch.Draw(metaballTarget, Vector2.Zero, Color.White);
				spriteBatch.End();
				device.SetRenderTarget(null);
			}
		}
 
		public override void Draw(SpriteBatch spriteBatch)
		{
			Color light = color;
			light.A = Convert.ToByte(0.7f);
			Color dark = Color.Black;

			if (emitSplashParticles)
			{
				// draw the particles 3 times to create a bevelling effect
				spriteBatch.Draw(particlesTarget, -Vector2.One, new Color(0.8f, 0.8f, 1f));
				spriteBatch.Draw(particlesTarget, Vector2.One, new Color(0f, 0f, 0.2f));
				spriteBatch.Draw(particlesTarget, Vector2.Zero, light);
			}

			// draw the waves
			pb.Begin(PrimitiveType.TriangleList);

			float bottom = area.Bottom;
			float scale = Scale;
			for (int i = 1; i < columns.Length; i++)
			{
				Vector2 p1 = new Vector2(area.X + (i - 1) * scale, columns[i - 1].Height);
				Vector2 p2 = new Vector2(area.X + i * scale, columns[i].Height);
				Vector2 p3 = new Vector2(p2.X, bottom);
				Vector2 p4 = new Vector2(p1.X, bottom);

				pb.AddVertex(p1 - GameWorld.Camera.Position + Neon.HalfScreen, light);
				pb.AddVertex(p2 - GameWorld.Camera.Position + Neon.HalfScreen, light);
				pb.AddVertex(p3 - GameWorld.Camera.Position + Neon.HalfScreen, dark);

				pb.AddVertex(p1 - GameWorld.Camera.Position + Neon.HalfScreen, light);
				pb.AddVertex(p3 - GameWorld.Camera.Position + Neon.HalfScreen, dark);
				pb.AddVertex(p4 - GameWorld.Camera.Position + Neon.HalfScreen, dark);
			}

			pb.End();
		}

		private bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			Splash(CoordinateConversion.worldToScreen(fixtureB.Body.Position.X), (float)Math.Pow(fixtureB.Body.LinearVelocity.Y, 3));
			return true;
		}

		private void onSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			Splash(CoordinateConversion.worldToScreen(fixtureB.Body.Position.X), -(float)Math.Pow(fixtureB.Body.LinearVelocity.Y, 3));
		}
	}
}
