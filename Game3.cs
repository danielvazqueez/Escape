/*
 * Luis Daniel Vázquez Peña - A01039545
 * Proyecto: Escape
 * Maestra: Yolanda Martínez 
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

#region Clase Background
class Background
{
    public Texture2D fondo;
    public Rectangle rectangle;
    public Background(Texture2D nuevoFondo, Rectangle newRectangle)
    {
        fondo = nuevoFondo;
        rectangle = newRectangle;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(fondo, rectangle, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch, Color temp)
    {
        spriteBatch.Draw(fondo, rectangle, temp);
    }
}

class Scrolling : Background
{
    public Scrolling(Texture2D nuevoFondo, Rectangle newRectangle) : base(nuevoFondo, newRectangle)
    {
    }

    public void Update(int temp)
    {
        rectangle.Y += temp;
    }
    
    public void UpdateX(int temp)
    {
        rectangle.X -= temp;
    }
}
#endregion

#region Objetos a dibujar
class ObjetosADibujar
{
    private int velX;
    private int velY;
    private bool atrapado;
    private int MaloOBueno;
    private Vector2 posicion;

    public ObjetosADibujar(int pX, int pY, int vX, int vY, int bom)
    {
        
        posicion = new Vector2(pX, pY);
        velX = vX;
        velY = vY;
        MaloOBueno = bom;
        atrapado = false;
    }

    public Vector2 Posicion
    {
        get { return posicion; }
    }
    public int MoB
    {
        get { return MaloOBueno; }
        set { MaloOBueno = value; }
    }

    public int PosicX
    {
        get { return (int)posicion.X; }
        set { posicion.X = value; }
    }
    public int PosicY
    {
        get { return (int)posicion.Y; }
        set { posicion.Y = value; }
    }
    public int VelX
    {
        get { return velX; }
        set { velX = value; }
    }
    public int VelY
    {
        get { return velY; }
        set { velY = value; }
    }
    public void addVelX()
    {
        posicion.X -= VelX;
    }
    public bool Atrapado
    {
        get { return atrapado; }
        set { atrapado = value; }
    }

    public void addVelY()
    {
        posicion.Y += VelY;
    }
    public Rectangle getArea(Texture2D temp)
    {
        Rectangle area = new Rectangle((int)posicion.X-temp.Width/2, (int)posicion.Y - temp.Height/2,
                                temp.Width, temp.Height);
        return area;
    }
}
#endregion
namespace Game3
{


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont fuente;
        Texture2D alien, luna, tierra, heart, bomba, cursor, bar, red, orange, gas, botonInicio, botonInicioH, logo, estrella, botonInstrucciones, botonInstruccionesH, botonSiguiente, botonSiguienteH, botonAtras, botonAtrasH, botonFin, botonFinH, botonCreditos, botonCreditosH;
        Texture2D botonVolver, botonVolverH;
        Texture2D[] Spaceship, spaceMuestra;
        int estrellaActual, spaceshipActual, finNivel;
        float rotation, rotacionEstrella, rotacionPiedra, sumEstrella;
        float speed;
        Vector2 position;
        Vector2 mousePosition;
        const int NUMESTRELLAS = 8;
        Background fondoInicio, fondoFinal, fondoNivel1, fondoNivel0, fondoNivel2, fondoNivel3, fondoTemp, fondoInstrucciones1, fondoInstrucciones2, fondoInstrucciones3, fondoInstrucciones4, fondoInstrucciones4Piedra, fondoInstrucciones4Vida, fondoInstrucciones4Gas, fondoInstrucciones4Estrella;
        Background fondoGameOver, fondoCreditos, fondoNivel4, fondoNivel5;
        Scrolling fondo1, fondo2, fondoLuna1, fondoLuna2;
        KeyboardState estadoTeclado;
        List<ObjetosADibujar> listaPelotas =
                        new List<ObjetosADibujar>();
        Random numRandom = new Random();
        double retrasoEntreTeclas, tiempoPelota, tiempoTotal, tiempoEstrella, tiempoSpaceship, tiempoGas, currentGas, timeGas, timeLife, tiempoLimite;
        int estadoJuego, seleccion;
        int currentHealth;
        bool acelerando, aparecerLuna, aparecerTierra;
        Color tinta;
        double transparencia;
        //Sonidos
        Song song;
        SoundEffect sonidoColision, sonidoClick, sonidoPuntos, sonidoDisparar, sonidoExplotar;
        bool hoverInicio, hoverInstrucciones, hoverSiguiente, hoverAtras, hoverFin, hoverCreditos;
        const int ancho = 1000;
        const int altura = 600;
        //Bullets
        List<Bullets> bullets = new List<Bullets>();
        //Mouse
        MouseState lastMouseState;
        MouseState mouse;

        //Estrellas necesarias
        int estrellasFaltantes;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            Spaceship = new Texture2D[6];
            spaceMuestra = new Texture2D[8];
            retrasoEntreTeclas = 0.2f;
            tiempoPelota = 0;
            tiempoTotal = 0;
            tiempoGas = 0;
            timeGas = 0;
            timeLife = 0;
            position = new Vector2(200, 200);
            rotation = 0;
            rotacionEstrella = 0;
            rotacionPiedra = 0;
            acelerando = false;
            finNivel = 0;
            speed = 4;
            tinta = new Color(255, 255, 255, 50);
            sumEstrella = 0.025f;
            estadoJuego = 0; //Inicia en la pantalla principal
            for (int i = 0; i < 5; i++)
            {
                ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(4) + 1, numRandom.Next(4) + 1, numRandom.Next(2));
                listaPelotas.Add(elemento);
            }
            base.Initialize();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }


        protected override void LoadContent()
        {
            #region Imagenes
            // Create a new SpriteBatch, which can be used to draw textures
            graphics.PreferredBackBufferWidth = ancho;
            graphics.PreferredBackBufferHeight = altura;
            graphics.ApplyChanges();
            fondoInstrucciones1 = new Background(Content.Load<Texture2D>("Instrucciones1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones2 = new Background(Content.Load<Texture2D>("Instrucciones2"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones3 = new Background(Content.Load<Texture2D>("Instrucciones3"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones4 = new Background(Content.Load<Texture2D>("Instrucciones4"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones4Piedra = new Background(Content.Load<Texture2D>("Instrucciones4Piedras"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones4Estrella = new Background(Content.Load<Texture2D>("Instrucciones4Estrella"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones4Vida = new Background(Content.Load<Texture2D>("Instrucciones4Vida"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInstrucciones4Gas = new Background(Content.Load<Texture2D>("Instrucciones4Gas"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoGameOver = new Background(Content.Load<Texture2D>("fondoGameOver"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoCreditos = new Background(Content.Load<Texture2D>("fondoCreditos"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoInicio = new Background(Content.Load<Texture2D>("Espacio"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoFinal = new Background(Content.Load<Texture2D>("FondoInicio"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel1 = new Background(Content.Load<Texture2D>("Mensaje1"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel0 = new Background(Content.Load<Texture2D>("Mensaje0"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel2 = new Background(Content.Load<Texture2D>("Mensaje2"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel3 = new Background(Content.Load<Texture2D>("Mensaje3"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel4 = new Background(Content.Load<Texture2D>("Mensaje4"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoNivel5 = new Background(Content.Load<Texture2D>("Mensaje5"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondo1 = new Scrolling(Content.Load<Texture2D>("Espacio"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondo2 = new Scrolling(Content.Load<Texture2D>("Espacio"), new Rectangle(0, -600, 1000, 600));
            fondoLuna1 = new Scrolling(Content.Load<Texture2D>("Moon"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            fondoLuna2 = new Scrolling(Content.Load<Texture2D>("Moon"), new Rectangle(1000, 0, 1000, 600));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fuente = Content.Load<SpriteFont>("Arial");
            heart = Content.Load<Texture2D>("Heart");
            bomba = Content.Load<Texture2D>("Bomba");
            cursor = Content.Load<Texture2D>("Mouse");
            estrella = Content.Load<Texture2D>("Star1");
            logo = Content.Load<Texture2D>("Logo");
            luna = Content.Load<Texture2D>("Luna");
            alien = Content.Load<Texture2D>("Alien");
            tierra = Content.Load<Texture2D>("Tierra");
            Spaceship[0] = Content.Load<Texture2D>("SpaceshipR4");
            Spaceship[1] = Content.Load<Texture2D>("SpaceshipR3");
            Spaceship[2] = Content.Load<Texture2D>("SpaceshipR2");
            Spaceship[3] = Content.Load<Texture2D>("SpaceshipR");
            Spaceship[4] = Content.Load<Texture2D>("SpaceshipR2");
            Spaceship[5] = Content.Load<Texture2D>("SpaceshipR3");
            spaceMuestra[0] = Content.Load<Texture2D>("SpaceshipR5G");
            spaceMuestra[1] = Content.Load<Texture2D>("SpaceshipR4G");
            spaceMuestra[2] = Content.Load<Texture2D>("SpaceshipR3G");
            spaceMuestra[3] = Content.Load<Texture2D>("SpaceshipR2G");
            spaceMuestra[4] = Content.Load<Texture2D>("SpaceshipRG");
            spaceMuestra[5] = Content.Load<Texture2D>("SpaceshipR2G");
            spaceMuestra[6] = Content.Load<Texture2D>("SpaceshipR3G");
            spaceMuestra[7] = Content.Load<Texture2D>("SpaceshipR4G");
            song = Content.Load<Song>("Believer - 8Bit");
            sonidoColision = Content.Load<SoundEffect>("Impact");
            sonidoClick = Content.Load<SoundEffect>("Click");
            sonidoDisparar = Content.Load<SoundEffect>("Shoot");
            sonidoPuntos = Content.Load<SoundEffect>("Positive");
            sonidoExplotar = Content.Load<SoundEffect>("Bomb");
            bar = Content.Load<Texture2D>("Bar");
            orange = Content.Load<Texture2D>("Orange");
            red = Content.Load<Texture2D>("Red");
            gas = Content.Load<Texture2D>("Gas-Icon");
            botonInicio = Content.Load<Texture2D>("BotonInicio");
            botonInicioH = Content.Load<Texture2D>("BotonInicioHover");
            botonInstrucciones = Content.Load<Texture2D>("BotonInstrucciones");
            botonInstruccionesH = Content.Load<Texture2D>("BotonInstruccionesHover");
            botonSiguiente = Content.Load<Texture2D>("BotonSiguiente");
            botonSiguienteH = Content.Load<Texture2D>("BotonSiguienteHover");
            botonAtras = Content.Load<Texture2D>("BotonAtras");
            botonAtrasH = Content.Load<Texture2D>("BotonAtrasHover");
            botonFin = Content.Load<Texture2D>("BotonFin");
            botonFinH = Content.Load<Texture2D>("BotonFinHover");
            botonCreditos = Content.Load<Texture2D>("BotonCreditos");
            botonCreditosH = Content.Load<Texture2D>("BotonCreditosHover");
            botonVolver = Content.Load<Texture2D>("BotonPlayAgain");
            botonVolverH = Content.Load<Texture2D>("BotonPlayAgainHover");
            // TODO: use this.Content to load your game content here
            #endregion
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //Inicio del codigo
            lastMouseState = mouse;
            mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);
            double transcurrido = gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 direction;
            rotacionEstrella += sumEstrella;
            rotacionPiedra += 0.1f;
            switch (estadoJuego) //Switch para los estados del juego
            {
                case 0: //En el inicio del juego
                    #region Inicio del juego
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(568, 210, botonInicio.Width, botonInicio.Height))) {
                        hoverInicio = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            estadoJuego = 10;
                            finNivel = 1;
                            fondoTemp = fondoNivel0;
                            estrellasFaltantes = 40;
                            transparencia = 0;
                            sonidoClick.Play();
                        }
                    } else
                    {
                        hoverInicio = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(568, 310, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverInstrucciones = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            estadoJuego = 11;
                            sonidoClick.Play();
                        }
                    } else
                    {
                        hoverInstrucciones = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(568, 410, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverCreditos = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 7;
                        }
                    } else
                    {
                        hoverCreditos = false;
                    }
                    

                    tiempoSpaceship += transcurrido;
                    if (tiempoSpaceship >= .10f)
                    {
                        spaceshipActual++;
                        if (spaceshipActual >= 8)
                            spaceshipActual = 0;
                        tiempoSpaceship = 0;
                    }
                    position.X = 246;
                    position.Y = 373;

                    //Spaceship moverse
                    direction = mousePosition - position;
                    direction.Normalize();

                    //Transparencia del Escape
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;

                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                    }
                    break;
                #endregion
                case 1:
                    #region Nivel 1
                    if (currentGas <= 0 || currentHealth <= 0)
                    {
                        estadoJuego = 6;
                    }

                    if (estrellasFaltantes <= 0)
                    {
                        estadoJuego = 10;
                        finNivel = 2;
                        fondoTemp = fondoNivel1;
                        estrellasFaltantes = 40;
                    }

                    estadoTeclado = Keyboard.GetState();
                    /*
                    if (estadoTeclado.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    */
                    if(estadoTeclado.IsKeyDown(Keys.Space))
                    {
                        speed = 7;
                        acelerando = true;
                    }
                    else
                    {
                        speed = 4;
                        acelerando = false;
                    }
                    

                    tiempoPelota += transcurrido;
                    tiempoTotal += transcurrido;
                    tiempoEstrella += transcurrido;
                    tiempoSpaceship += transcurrido;
                    tiempoGas += transcurrido;
                    timeGas += transcurrido;
                    timeLife += transcurrido;

                    direction = mousePosition - position;
                    direction.Normalize();

                    if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        Shoot();
                        sonidoDisparar.Play();
                    }
                        

                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                        position += direction * speed;

                    }

                    if (tiempoGas > retrasoEntreTeclas)
                    {
                        if (acelerando)
                            currentGas -= 2.5;
                        else
                            currentGas -= .75;
                        tiempoGas = 0;
                    }

                    fondo1.Update(3); //Scroll background
                    fondo2.Update(3); //Scroll background



                    //Cambiar la imagen del spaceship para cambiar el fuego
                    if(acelerando)
                    {
                        if(tiempoSpaceship >= .10f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }
                    else
                    {
                        if(tiempoSpaceship >= .20f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }

                    if (tiempoEstrella > .15f)
                    {
                     //   estrellaActual++;
                        if (estrellaActual >= NUMESTRELLAS)
                            estrellaActual = 0;
                        tiempoEstrella = 0;
                    }

                    if (tiempoPelota >= retrasoEntreTeclas)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 1, numRandom.Next(5) + 1, numRandom.Next(2));
                        listaPelotas.Add(elemento);
                        tiempoPelota = 0;
                    }
                    if (timeGas > 4f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 5, numRandom.Next(5) + 2, 2);
                        listaPelotas.Add(elemento);
                        timeGas = 0;
                    }
                    if (timeLife > 5f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 5, numRandom.Next(5) + 2, 3);
                        listaPelotas.Add(elemento);
                        timeLife = 0;
                    }



                    //No se salga por la izquierda
                    if ((int)position.X - Spaceship[0].Width / 2 < 0)
                        position.X = Spaceship[0].Width / 2;

                    //No se salga por abajo
                    if (position.Y + Spaceship[0].Height / 2 > graphics.GraphicsDevice.Viewport.Height)
                        position.Y = graphics.GraphicsDevice.Viewport.Height - Spaceship[0].Height / 2;

                    //No se salga por arriba
                    if (position.Y - Spaceship[0].Height / 2 < 0)
                        position.Y = Spaceship[0].Height / 2;

                    //No se salga por la derecha 
                    if (position.X + Spaceship[0].Width / 2 > graphics.GraphicsDevice.Viewport.Width)
                        position.X = graphics.GraphicsDevice.Viewport.Width - Spaceship[0].Width / 2;

                    foreach (ObjetosADibujar obj in listaPelotas)
                        obj.addVelY();

                    Rectangle areaSpaceship = new Rectangle((int)position.X - Spaceship[0].Width / 2 + 10, (int)position.Y - Spaceship[0].Height / 2 + 5,
                                                    Spaceship[0].Width - 10, Spaceship[0].Height - 5);

                    foreach (ObjetosADibujar obj in listaPelotas) {
                        if (obj.MoB == 0)
                        {
                            if (obj.getArea(estrella).Intersects(areaSpaceship))
                            {
                                estrellasFaltantes--;
                                obj.Atrapado = true;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 1)
                        {
                            if (obj.getArea(bomba).Intersects(areaSpaceship))
                            {
                                currentHealth -= 20;
                                obj.Atrapado = true;
                                sonidoColision.Play();
                            }
                            foreach(Bullets bullet in bullets)
                            {
                                if (obj.getArea(bomba).Intersects(bullet.getArea(bullet.texture)))
                                {
                                    bullet.isVisible = false;
                                    obj.Atrapado = true;
                                    sonidoExplotar.Play();
                                }
                            }
                        }
                        else if (obj.MoB == 2)
                        {
                            if (obj.getArea(gas).Intersects(areaSpaceship))
                            {
                                currentGas += 30;
                                obj.Atrapado = true;
                                if (currentGas > 245)
                                    currentGas = 245;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 3)
                        {
                            if (obj.getArea(heart).Intersects(areaSpaceship))
                            {
                                currentHealth += 15;
                                obj.Atrapado = true;
                                if (currentHealth > 245)
                                    currentHealth = 245;
                                sonidoPuntos.Play();
                            }
                        }
                    }

                    //Quitar objetos que ya no estan en la pantalla
                    listaPelotas.RemoveAll(obj => obj.Atrapado == true);
                    listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);
                    
                    UpdateBullets();
                    //Scrolling background
                    if (fondo1.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo1.rectangle.Y = fondo2.rectangle.Y - 600;
                    }
                    if (fondo2.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo2.rectangle.Y = fondo1.rectangle.Y - 600;
                    }
                    break;
                #endregion
                case 2:
                    #region Nivel 2
                    if (currentGas <= 0 || currentHealth <= 0)
                    {
                        estadoJuego = 6;
                    }

                    if (estrellasFaltantes <= 0)
                    {
                        estadoJuego = 10;
                        finNivel = 3;
                        fondoTemp = fondoNivel2;
                        estrellasFaltantes = 35;
                    }

                    estadoTeclado = Keyboard.GetState();
                    /*
                    if (estadoTeclado.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    */
                    if (estadoTeclado.IsKeyDown(Keys.Space))
                    {
                        speed = 7;
                        acelerando = true;
                    }
                    else
                    {
                        speed = 4;
                        acelerando = false;
                    }

                    //Tiempos
                    tiempoPelota += transcurrido;
                    tiempoTotal += transcurrido;
                    tiempoEstrella += transcurrido;
                    tiempoSpaceship += transcurrido;
                    tiempoGas += transcurrido;
                    timeGas += transcurrido;
                    timeLife += transcurrido;

                    direction = mousePosition - position;
                    direction.Normalize();

                    if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        Shoot();
                        sonidoDisparar.Play();
                    }

                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                        position += direction * speed;
                    }

                    if (tiempoGas > retrasoEntreTeclas)
                    {
                        if (acelerando)
                            currentGas -= 2;
                        else
                            currentGas -= .75;
                        tiempoGas = 0;
                    }

                    fondo1.Update(3); //Scroll background
                    fondo2.Update(3); //Scroll background


                    //Cambiar la imagen del spaceship para cambiar el fuego
                    if (acelerando)
                    {
                        if (tiempoSpaceship >= .10f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }
                    else
                    {
                        if (tiempoSpaceship >= .20f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }

                    if (tiempoPelota >= retrasoEntreTeclas)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 1, numRandom.Next(5) + 6, numRandom.Next(2));
                        listaPelotas.Add(elemento);
                        tiempoPelota = 0;
                    }
                    if (timeGas > 4f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(5) + 7, numRandom.Next(4) + 9, 2);
                        listaPelotas.Add(elemento);
                        timeGas = 0;
                    }
                    if (timeLife > 5f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 5, numRandom.Next(4) + 9, 3);
                        listaPelotas.Add(elemento);
                        timeLife = 0;
                    }



                    //No se salga por la izquierda
                    if ((int)position.X - Spaceship[0].Width / 2 < 0)
                        position.X = Spaceship[0].Width / 2;

                    //No se salga por abajo
                    if (position.Y + Spaceship[0].Height / 2 > graphics.GraphicsDevice.Viewport.Height)
                        position.Y = graphics.GraphicsDevice.Viewport.Height - Spaceship[0].Height / 2;

                    //No se salga por arriba
                    if (position.Y - Spaceship[0].Height / 2 < 0)
                        position.Y = Spaceship[0].Height / 2;

                    //No se salga por la derecha 
                    if (position.X + Spaceship[0].Width / 2 > graphics.GraphicsDevice.Viewport.Width)
                        position.X = graphics.GraphicsDevice.Viewport.Width - Spaceship[0].Width / 2;

                    foreach (ObjetosADibujar obj in listaPelotas)
                        obj.addVelY();

                    areaSpaceship = new Rectangle((int)position.X - Spaceship[0].Width / 2 + 10, (int)position.Y - Spaceship[0].Height / 2 + 5,
                                                    Spaceship[0].Width - 10, Spaceship[0].Height - 5);

                    foreach (ObjetosADibujar obj in listaPelotas)
                    {
                        if (obj.MoB == 0)
                        {
                            if (obj.getArea(estrella).Intersects(areaSpaceship))
                            {
                                estrellasFaltantes--;
                                obj.Atrapado = true;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 1)
                        {
                            if (obj.getArea(bomba).Intersects(areaSpaceship))
                            {
                                currentHealth -= 20;
                                obj.Atrapado = true;
                                sonidoColision.Play();
                            }
                            foreach (Bullets bullet in bullets)
                            {
                                if (obj.getArea(bomba).Intersects(bullet.getArea(bullet.texture)))
                                {
                                    bullet.isVisible = false;
                                    obj.Atrapado = true;
                                    sonidoExplotar.Play();
                                }
                            }
                        }
                        else if (obj.MoB == 2)
                        {
                            if (obj.getArea(gas).Intersects(areaSpaceship))
                            {
                                currentGas += 30;
                                obj.Atrapado = true;
                                if (currentGas > 245)
                                    currentGas = 245;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 3)
                        {
                            if (obj.getArea(heart).Intersects(areaSpaceship))
                            {
                                currentHealth += 15;
                                obj.Atrapado = true;
                                if (currentHealth > 245)
                                    currentHealth = 245;
                                sonidoPuntos.Play();
                            }
                        }
                    }

                    //Quitar objetos que ya no estan en la pantalla
                    listaPelotas.RemoveAll(obj => obj.Atrapado == true);
                    listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);

                    UpdateBullets();
                    //Scrolling background
                    if (fondo1.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo1.rectangle.Y = fondo2.rectangle.Y - 600;
                    }
                    if (fondo2.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo2.rectangle.Y = fondo1.rectangle.Y - 600;
                    }
                    break;
                #endregion
                case 3:
                    #region Nivel 3
                    if (currentGas <= 0 || currentHealth <= 0)
                    {
                        estadoJuego = 6;
                    }

                    if (estrellasFaltantes <= 0)
                    {
                        aparecerLuna = true;
                    }

                    estadoTeclado = Keyboard.GetState();

                    if (estadoTeclado.IsKeyDown(Keys.Space))
                    {
                        speed = 7;
                        acelerando = true;
                    }
                    else
                    {
                        speed = 4;
                        acelerando = false;
                    }

                    //Tiempos
                    tiempoPelota += transcurrido;
                    tiempoTotal += transcurrido;
                    tiempoEstrella += transcurrido;
                    tiempoSpaceship += transcurrido;
                    tiempoGas += transcurrido;
                    timeGas += transcurrido;
                    timeLife += transcurrido;

                    direction = mousePosition - position;
                    direction.Normalize();

                    if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        Shoot();
                        sonidoDisparar.Play();
                    }
                        

                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                        position += direction * speed;
                    }
                    if (tiempoGas > retrasoEntreTeclas)
                    {
                        if (acelerando)
                            currentGas -= 1.75;
                        else
                            currentGas -= 0.75;
                        tiempoGas = 0;
                    }
                    fondo1.Update(3); //Scroll background
                    fondo2.Update(3); //Scroll background


                    //Cambiar la imagen del spaceship para cambiar el fuego
                    if (acelerando)
                    {
                        if (tiempoSpaceship >= .10f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }
                    else
                    {
                        if (tiempoSpaceship >= .20f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }

                    if (tiempoEstrella > .15f)
                    {
                        estrellaActual++;
                        if (estrellaActual >= NUMESTRELLAS)
                            estrellaActual = 0;
                        tiempoEstrella = 0;
                    }

                    if (tiempoPelota >= retrasoEntreTeclas)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 1, numRandom.Next(5) + 4, numRandom.Next(2));
                        listaPelotas.Add(elemento);
                        tiempoPelota = 0;
                    }


                    //No se salga por la izquierda
                    if ((int)position.X - Spaceship[0].Width / 2 < 0)
                        position.X = Spaceship[0].Width / 2;

                    //No se salga por abajo
                    if (position.Y + Spaceship[0].Height / 2 > graphics.GraphicsDevice.Viewport.Height)
                        position.Y = graphics.GraphicsDevice.Viewport.Height - Spaceship[0].Height / 2;

                    //No se salga por arriba
                    if (position.Y - Spaceship[0].Height / 2 < 0)
                        position.Y = Spaceship[0].Height / 2;

                    //No se salga por la derecha 
                    if (position.X + Spaceship[0].Width / 2 > graphics.GraphicsDevice.Viewport.Width)
                        position.X = graphics.GraphicsDevice.Viewport.Width - Spaceship[0].Width / 2;

                    foreach (ObjetosADibujar obj in listaPelotas)
                        obj.addVelY();

                    areaSpaceship = new Rectangle((int)position.X - Spaceship[0].Width / 2 + 10, (int)position.Y - Spaceship[0].Height / 2 + 5,
                                                    Spaceship[0].Width - 10, Spaceship[0].Height - 5);

                    if (aparecerLuna && new Rectangle(258, 0, luna.Width, luna.Height).Intersects(areaSpaceship))
                    {
                        estadoJuego = 10;
                        finNivel = 4;
                        fondoTemp = fondoNivel3;
                        estrellasFaltantes = 15;
                    }

                    foreach (ObjetosADibujar obj in listaPelotas)
                    {
                        if (obj.MoB == 0)
                        {
                            if (obj.getArea(estrella).Intersects(areaSpaceship))
                            {
                                estrellasFaltantes--;
                                obj.Atrapado = true;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 1)
                        {
                            if (obj.getArea(bomba).Intersects(areaSpaceship))
                            {
                                currentHealth -= 20;
                                obj.Atrapado = true;
                                sonidoColision.Play();
                            }
                            foreach (Bullets bullet in bullets)
                            {
                                if (obj.getArea(bomba).Intersects(bullet.getArea(bullet.texture)))
                                {
                                    bullet.isVisible = false;
                                    obj.Atrapado = true;
                                    sonidoExplotar.Play();
                                }
                            }
                        }
                    }

                    //Quitar objetos que ya no estan en la pantalla
                    listaPelotas.RemoveAll(obj => obj.Atrapado == true);
                    listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);

                    UpdateBullets();
                    //Scrolling background
                    if (fondo1.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo1.rectangle.Y = fondo2.rectangle.Y - 600;
                    }
                    if (fondo2.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo2.rectangle.Y = fondo1.rectangle.Y - 600;
                    }
                    break;
                #endregion
                case 4:
                    #region Nivel 4
                    fondoLuna1.UpdateX(3);
                    fondoLuna2.UpdateX(3);

                    if (currentGas <= 0 || currentHealth <= 0)
                    {
                        estadoJuego = 6;
                    }

                    if (estrellasFaltantes <= 0)
                    {
                        estadoJuego = 10;
                        finNivel = 5;
                        fondoTemp = fondoNivel4;
                        estrellasFaltantes = 40;
                    }

                    estadoTeclado = Keyboard.GetState();
                    /*
                    if (estadoTeclado.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    */
                    if (estadoTeclado.IsKeyDown(Keys.Space))
                    {
                        speed = 7;
                        acelerando = true;
                    }
                    else
                    {
                        speed = 4;
                        acelerando = false;
                    }

                    //Tiempos
                    tiempoPelota += transcurrido;
                    tiempoEstrella += transcurrido;
                    tiempoSpaceship += transcurrido;
                    tiempoGas += transcurrido;
                    timeGas += transcurrido;
                    timeLife += transcurrido;

                    direction = mousePosition - position;
                    direction.Normalize();

                    if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        Shoot();
                        sonidoDisparar.Play();
                    }


                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                        position += direction * speed;
                    }

                    if (tiempoGas > retrasoEntreTeclas)
                    {
                        if (acelerando)
                            currentGas -= 2.5;
                        else
                            currentGas -= 1;
                        tiempoGas = 0;
                    }

                    //Cambiar la imagen del spaceship para cambiar el fuego
                    if (acelerando)
                    {
                        if (tiempoSpaceship >= .10f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }
                    else
                    {
                        if (tiempoSpaceship >= .20f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }

                    if (tiempoPelota > 1f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(150)+1000, numRandom.Next(530) + 20, numRandom.Next(4) + 7, numRandom.Next(4) + 9, 4);
                        listaPelotas.Add(elemento);
                        tiempoPelota = 0;
                    }

                    //No se salga por la izquierda
                    if ((int)position.X - Spaceship[0].Width / 2 < 0)
                        position.X = Spaceship[0].Width / 2;

                    //No se salga por abajo
                    if (position.Y + Spaceship[0].Height / 2 > graphics.GraphicsDevice.Viewport.Height)
                        position.Y = graphics.GraphicsDevice.Viewport.Height - Spaceship[0].Height / 2;

                    //No se salga por arriba
                    if (position.Y - Spaceship[0].Height / 2 < 0)
                        position.Y = Spaceship[0].Height / 2;

                    //No se salga por la derecha 
                    if (position.X + Spaceship[0].Width / 2 > graphics.GraphicsDevice.Viewport.Width)
                        position.X = graphics.GraphicsDevice.Viewport.Width - Spaceship[0].Width / 2;

                    foreach (ObjetosADibujar obj in listaPelotas)
                        if (obj.MoB == 4)
                            obj.addVelX();

                    areaSpaceship = new Rectangle((int)position.X - Spaceship[0].Width / 2 + 10, (int)position.Y - Spaceship[0].Height / 2 + 5,
                                                    Spaceship[0].Width - 10, Spaceship[0].Height - 5);

                    foreach (ObjetosADibujar obj in listaPelotas)
                    {
                        if (obj.MoB == 4)
                        {
                            foreach (Bullets bullet in bullets)
                            {
                                if (obj.getArea(alien).Intersects(bullet.getArea(bullet.texture)))
                                {
                                    bullet.isVisible = false;
                                    obj.MoB = 0;
                                    sonidoExplotar.Play();

                                }
                            }
                            if (obj.getArea(alien).Intersects(areaSpaceship))
                            {
                                obj.Atrapado = true;
                                currentHealth -= 65;
                                sonidoColision.Play();
                            }
                        }
                        else if(obj.MoB == 0)
                        {
                            if (obj.getArea(estrella).Intersects(areaSpaceship))
                            {
                                estrellasFaltantes--;
                                obj.Atrapado = true;
                                sonidoPuntos.Play();
                            }
                        }
                    }

                    //Quitar objetos que ya no estan en la pantalla
                    listaPelotas.RemoveAll(obj => obj.Atrapado == true);
                    listaPelotas.RemoveAll(obj => obj.PosicX + alien.Width < 0);

                    UpdateBullets();
                    if (fondoLuna1.rectangle.X + 1000 <= 0)
                    {
                        fondoLuna1.rectangle.X = fondoLuna2.rectangle.X + 1000;
                    }
                    if (fondoLuna2.rectangle.X + 1000 <= 0)
                    {
                        fondoLuna2.rectangle.X = fondoLuna1.rectangle.Y + 1000;
                    }
                    break;
                    #endregion
                case 5:
                    #region Nivel 5
                    if (currentGas <= 0 || currentHealth <= 0)
                    {
                        estadoJuego = 6;
                    }

                    if (estrellasFaltantes <= 0)
                    {
                        aparecerTierra = true;
                    }

                    estadoTeclado = Keyboard.GetState();
                    /*
                    if (estadoTeclado.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }
                    */
                    if (estadoTeclado.IsKeyDown(Keys.Space))
                    {
                        speed = 7;
                        acelerando = true;
                    }
                    else
                    {
                        speed = 4;
                        acelerando = false;
                    }


                    tiempoPelota += transcurrido;
                    tiempoTotal += transcurrido;
                    tiempoEstrella += transcurrido;
                    tiempoSpaceship += transcurrido;
                    tiempoGas += transcurrido;
                    timeGas += transcurrido;
                    timeLife += transcurrido;

                    direction = mousePosition - position;
                    direction.Normalize();

                    if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        Shoot();
                        sonidoDisparar.Play();
                    }

                    //Condicion que checa que el spaceship deberia moverse
                    if (Vector2.Distance(mousePosition, position) > 3)
                    {
                        rotation = (float)Math.Atan2(
                                    (double)direction.Y,
                                    (double)direction.X);
                        position += direction * speed;

                    }

                    if (tiempoGas > retrasoEntreTeclas)
                    {
                        if (acelerando)
                            currentGas -= 2.5;
                        else
                            currentGas -= 1;
                        tiempoGas = 0;
                    }

                    fondo1.Update(3); //Scroll background
                    fondo2.Update(3); //Scroll background



                    //Cambiar la imagen del spaceship para cambiar el fuego
                    if (acelerando)
                    {
                        if (tiempoSpaceship >= .10f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }
                    else
                    {
                        if (tiempoSpaceship >= .20f)
                        {
                            spaceshipActual++;
                            if (spaceshipActual >= 6)
                                spaceshipActual = 0;
                            tiempoSpaceship = 0;
                        }
                    }

                    if (tiempoPelota >= retrasoEntreTeclas)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 1, numRandom.Next(5) + 6, numRandom.Next(2));
                        listaPelotas.Add(elemento);
                        tiempoPelota = 0;
                    }
                    if (timeGas > 4f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 5, numRandom.Next(5) + 9, 2);
                        listaPelotas.Add(elemento);
                        timeGas = 0;
                    }
                    if (timeLife > 5f)
                    {
                        ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, numRandom.Next(7) + 5, numRandom.Next(5) + 9, 3);
                        listaPelotas.Add(elemento);
                        timeLife = 0;
                    }



                    //No se salga por la izquierda
                    if ((int)position.X - Spaceship[0].Width / 2 < 0)
                        position.X = Spaceship[0].Width / 2;

                    //No se salga por abajo
                    if (position.Y + Spaceship[0].Height / 2 > graphics.GraphicsDevice.Viewport.Height)
                        position.Y = graphics.GraphicsDevice.Viewport.Height - Spaceship[0].Height / 2;

                    //No se salga por arriba
                    if (position.Y - Spaceship[0].Height / 2 < 0)
                        position.Y = Spaceship[0].Height / 2;

                    //No se salga por la derecha 
                    if (position.X + Spaceship[0].Width / 2 > graphics.GraphicsDevice.Viewport.Width)
                        position.X = graphics.GraphicsDevice.Viewport.Width - Spaceship[0].Width / 2;

                    foreach (ObjetosADibujar obj in listaPelotas)
                        obj.addVelY();

                    areaSpaceship = new Rectangle((int)position.X - Spaceship[0].Width / 2 + 10, (int)position.Y - Spaceship[0].Height / 2 + 5,
                                                    Spaceship[0].Width - 10, Spaceship[0].Height - 5);

                    if (aparecerTierra && new Rectangle(100, 0, tierra.Width, tierra.Height).Intersects(areaSpaceship))
                    {
                        estadoJuego = 10;
                        finNivel = 8;
                        fondoTemp = fondoNivel5;
                    }

                    foreach (ObjetosADibujar obj in listaPelotas)
                    {
                        if (obj.MoB == 0)
                        {
                            if (obj.getArea(estrella).Intersects(areaSpaceship))
                            {
                                estrellasFaltantes--;
                                obj.Atrapado = true;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 1)
                        {
                            if (obj.getArea(bomba).Intersects(areaSpaceship))
                            {
                                currentHealth -= 20;
                                obj.Atrapado = true;
                                sonidoColision.Play();
                            }
                            foreach (Bullets bullet in bullets)
                            {
                                if (obj.getArea(bomba).Intersects(bullet.getArea(bullet.texture)))
                                {
                                    bullet.isVisible = false;
                                    obj.Atrapado = true;
                                    sonidoExplotar.Play();
                                }
                            }
                        }
                        else if (obj.MoB == 2)
                        {
                            if (obj.getArea(gas).Intersects(areaSpaceship))
                            {
                                currentGas += 30;
                                obj.Atrapado = true;
                                if (currentGas > 245)
                                    currentGas = 245;
                                sonidoPuntos.Play();
                            }
                        }
                        else if (obj.MoB == 3)
                        {
                            if (obj.getArea(heart).Intersects(areaSpaceship))
                            {
                                currentHealth += 15;
                                obj.Atrapado = true;
                                if (currentHealth > 245)
                                    currentHealth = 245;
                                sonidoPuntos.Play();
                            }
                        }
                    }

                    //Quitar objetos que ya no estan en la pantalla
                    listaPelotas.RemoveAll(obj => obj.Atrapado == true);
                    listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);

                    UpdateBullets();
                    //Scrolling background
                    if (fondo1.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo1.rectangle.Y = fondo2.rectangle.Y - 600;
                    }
                    if (fondo2.rectangle.Y > GraphicsDevice.Viewport.Height)
                    {
                        fondo2.rectangle.Y = fondo1.rectangle.Y - 600;
                    }
                    break;
                #endregion
                case 6:
                    #region Game Over
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            estadoJuego = 0;
                            sonidoClick.Play();
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverFin = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            Exit();
                        }
                    }
                    else
                    {
                        hoverFin = false;
                    }
                    break;
                    #endregion
                case 7:
                    #region Creditos
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 0;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }
                    break;
                #endregion
                case 8:
                    #region Ganador
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            estadoJuego = 0;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverFin = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            Exit();
                        }
                    }
                    else
                    {
                        hoverFin = false;
                    }
                    break;
                    #endregion
                case 10:
                    #region Mostrar pantalla negra
                    tiempoLimite += transcurrido;
                    transparencia += 0.5;
                    if(tiempoLimite > 5f)
                    {
                        estadoJuego = finNivel;
                        currentGas = 245;
                        currentHealth = 245;
                        transparencia = 0;
                        spaceshipActual = 0;
                        tiempoLimite = 0;
                    }
                    listaPelotas.RemoveAll(obj => obj.Atrapado == false);
                    break;
                #endregion
                case 11:
                    #region Primer ventana instruccion
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverSiguiente = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 12;
                        }
                    }
                    else
                    {
                        hoverSiguiente = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 0;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }
                    break;
                    #endregion
                case 12:
                    #region Segunda ventana instruccion
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverSiguiente = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 13;
                        }
                    }
                    else
                    {
                        hoverSiguiente = false;
                    }
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 11;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }
                    break;
                    #endregion
                case 13:
                    #region Tercer ventana instruccion
                    transparencia += 1.5;
                    if (transparencia > 200)
                        transparencia = 10;
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverSiguiente = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 14;
                        }
                    }
                    else
                    {
                        hoverSiguiente = false;
                    }
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 12;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }
                    break;
                    #endregion
                case 14:
                    #region Cuarta ventana instruccion
                    tiempoPelota += transcurrido;
                    transparencia += 1.5;
                    foreach (ObjetosADibujar obj in listaPelotas)
                        obj.addVelY();

                    listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);

                    if (transparencia > 200)
                        transparencia = 10;
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(680, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverFin = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 0;
                        }
                    }
                    else
                    {
                        hoverFin = false;
                    }
                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(12, 482, botonInicio.Width, botonInicio.Height)))
                    {
                        hoverAtras = true;
                        if (mouse.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                        {
                            sonidoClick.Play();
                            estadoJuego = 13;
                        }
                    }
                    else
                    {
                        hoverAtras = false;
                    }

                    if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(126, 250, 106, 78)))
                    {
                        seleccion = 1;
                        if (tiempoPelota >= 0.15f)
                        {
                            ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, 1, numRandom.Next(8) + 1, 1);
                            listaPelotas.Add(elemento);
                            tiempoPelota = 0;
                        }
                    }
                    else if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(329, 237, 94, 91)))
                    {
                        seleccion = 2;
                        if (tiempoPelota >= 0.15f)
                        {
                            ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, 1, numRandom.Next(8) + 1, 0);
                            listaPelotas.Add(elemento);
                            tiempoPelota = 0;
                        }
                    }
                    else if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(539, 251, 73, 72)))
                    {
                        seleccion = 3;
                        if (tiempoPelota >= 0.15f)
                        {
                            ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, 1, numRandom.Next(8) + 1, 2);
                            listaPelotas.Add(elemento);
                            tiempoPelota = 0;
                        }
                    }
                    else if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(new Rectangle(755, 258, 76, 65)))
                    {
                        seleccion = 4;
                        if (tiempoPelota >= 0.15f)
                        {
                            ObjetosADibujar elemento = new ObjetosADibujar(numRandom.Next(GraphicsDevice.Viewport.Width - 50), numRandom.Next(250) * -1, 1, numRandom.Next(8) + 1, 3);
                            listaPelotas.Add(elemento);
                            tiempoPelota = 0;
                        }
                    }
                    else
                    {
                        seleccion = 0;
                        listaPelotas.RemoveAll(obj => obj.PosicY < 600);
                        listaPelotas.RemoveAll(obj => obj.PosicY > GraphicsDevice.Viewport.Height);
                    }
                    break;
                    #endregion

            }

            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            foreach(Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, position) > 600)
                    bullet.isVisible = false;
            }
            bullets.RemoveAll(obj => obj.isVisible == false);

        }

        public void Shoot()
        {
            Bullets newBullet = new Bullets(Content.Load<Texture2D>("Bullet"));
            newBullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 10f;
            newBullet.position = position + newBullet.velocity * 5;
            newBullet.isVisible = true;

            if (bullets.Count < 50)
                bullets.Add(newBullet);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 









            /// METODO DRAWWW
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (estadoJuego)
            {
                case 0:
                    #region Draw Nivel 0
                    fondoInicio.Draw(spriteBatch);
                    spriteBatch.Draw(spaceMuestra[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        spaceMuestra[0].Width / 2,
                                        spaceMuestra[0].Height / 2),
                                    1.0f,
                                    SpriteEffects.None,
                                    1.0f);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    if (!hoverInicio)
                        spriteBatch.Draw(botonInicio, new Rectangle(568, 210, botonInicio.Width, botonInicio.Height), Color.White);
                    else 
                        spriteBatch.Draw(botonInicioH, new Rectangle(568, 210, botonInicio.Width, botonInicio.Height), Color.White);

                    if (!hoverInstrucciones)
                        spriteBatch.Draw(botonInstrucciones, new Rectangle(568, 310, botonInstrucciones.Width, botonInstrucciones.Height), Color.White);
                    else
                        spriteBatch.Draw(botonInstruccionesH, new Rectangle(568, 310, botonInstrucciones.Width, botonInstrucciones.Height), Color.White);
                    if (!hoverCreditos)
                        spriteBatch.Draw(botonCreditos, new Rectangle(568, 410, botonCreditos.Width, botonCreditos.Height), Color.White);
                    else
                        spriteBatch.Draw(botonCreditosH, new Rectangle(568, 410, botonCreditos.Width, botonCreditos.Height), Color.White);
                  
                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                #endregion
                case 1: 
                    #region Draw Nivel 1
                    fondo1.Draw(spriteBatch);
                    fondo2.Draw(spriteBatch);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    spriteBatch.Draw(Spaceship[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        Spaceship[0].Width / 2,
                                        Spaceship[0].Height / 2),
                                    1.2f,
                                    SpriteEffects.None,
                                    1.0f);
                    foreach (ObjetosADibujar obj in listaPelotas)
                        if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                        else if (obj.MoB == 1)
                        {
                            spriteBatch.Draw(bomba,
                                        obj.Posicion,
                                        null,
                                        Color.White,
                                        rotacionPiedra,
                                        new Vector2(bomba.Width / 2, bomba.Height / 2),
                                            1.0f,
                                            SpriteEffects.None,
                                            1.0f);
                        }
                        else if(obj.MoB == 2)
                        {
                            spriteBatch.Draw(gas, new Rectangle(obj.PosicX,obj.PosicY, 40, 40), Color.White);
                        }
                        else if(obj.MoB == 3)
                        {
                            spriteBatch.Draw(heart, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }

                    foreach (Bullets bullet in bullets)
                        bullet.Draw(spriteBatch);

                    spriteBatch.Draw(gas, new Rectangle(1, 55, 30, 30), tinta);
                    spriteBatch.Draw(heart, new Rectangle(1, 15, 30, 30), tinta);
                    spriteBatch.Draw(red, new Rectangle(38, 13, currentHealth, 34), tinta);
                    spriteBatch.Draw(orange, new Rectangle(38, 53, (int)currentGas, 34), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 10, 250, 40), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 50, 250, 40), tinta);
                    spriteBatch.DrawString(fuente, "Estrellas faltantes = " + estrellasFaltantes.ToString(),
                                        new Vector2(345, 15), Color.White);
                    break;
                #endregion
                case 2:
                    #region Draw Nivel 2
                    fondo1.Draw(spriteBatch);
                    fondo2.Draw(spriteBatch);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    spriteBatch.Draw(Spaceship[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        Spaceship[0].Width / 2,
                                        Spaceship[0].Height / 2),
                                    1.2f,
                                    SpriteEffects.None,
                                    1.0f);
                    foreach (ObjetosADibujar obj in listaPelotas)
                        if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                        else if (obj.MoB == 1)
                        {
                            spriteBatch.Draw(bomba,
                                        obj.Posicion,
                                        null,
                                        Color.White,
                                        rotacionPiedra,
                                        new Vector2(bomba.Width / 2, bomba.Height / 2),
                                            1.0f,
                                            SpriteEffects.None,
                                            1.0f);
                        }
                        else if (obj.MoB == 2)
                        {
                            spriteBatch.Draw(gas, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }
                        else if (obj.MoB == 3)
                        {
                            spriteBatch.Draw(heart, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }

                    foreach (Bullets bullet in bullets)
                        bullet.Draw(spriteBatch);

                    spriteBatch.Draw(gas, new Rectangle(1, 55, 30, 30), tinta);
                    spriteBatch.Draw(heart, new Rectangle(1, 15, 30, 30), tinta);
                    spriteBatch.Draw(red, new Rectangle(38, 13, currentHealth, 34), tinta);
                    spriteBatch.Draw(orange, new Rectangle(38, 53, (int)currentGas, 34), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 10, 250, 40), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 50, 250, 40), tinta);
                    spriteBatch.DrawString(fuente, "Estrellas faltantes = " + estrellasFaltantes.ToString(),
                                        new Vector2(345, 15), Color.White);
                    break;
                #endregion
                case 3:
                    #region Draw Nivel 3
                    fondo1.Draw(spriteBatch);
                    fondo2.Draw(spriteBatch);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    spriteBatch.Draw(Spaceship[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        Spaceship[0].Width / 2,
                                        Spaceship[0].Height / 2),
                                    1.2f,
                                    SpriteEffects.None,
                                    1.0f);
                    foreach (ObjetosADibujar obj in listaPelotas) {
                        if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                        else if (obj.MoB == 1)
                        {
                            spriteBatch.Draw(bomba,
                                        obj.Posicion,
                                        null,
                                        Color.White,
                                        rotacionPiedra,
                                        new Vector2(bomba.Width / 2, bomba.Height / 2),
                                            1.0f,
                                            SpriteEffects.None,
                                            1.0f);
                        }
                        else if (obj.MoB == 2)
                        {
                            spriteBatch.Draw(gas, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }
                        else if (obj.MoB == 3)
                        {
                            spriteBatch.Draw(heart, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }
                    }
                    foreach (Bullets bullet in bullets)
                        bullet.Draw(spriteBatch);

                    spriteBatch.Draw(gas, new Rectangle(1, 55, 30, 30), tinta);
                    spriteBatch.Draw(heart, new Rectangle(1, 15, 30, 30), tinta);
                    spriteBatch.Draw(red, new Rectangle(38, 13, currentHealth, 34), tinta);
                    spriteBatch.Draw(orange, new Rectangle(38, 53, (int)currentGas, 34), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 10, 250, 40), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 50, 250, 40), tinta);

                    if (aparecerLuna)
                        spriteBatch.Draw(luna, new Rectangle(258, 0, luna.Width, luna.Height), Color.White);
                    else
                        spriteBatch.DrawString(fuente, "Estrellas faltantes = " + estrellasFaltantes.ToString(),
                                        new Vector2(345, 15), Color.White);
                    break;
                #endregion
                case 4:
                    #region Draw Nivel 4
                    fondoLuna1.Draw(spriteBatch);
                    fondoLuna2.Draw(spriteBatch);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    spriteBatch.Draw(Spaceship[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        Spaceship[0].Width / 2,
                                        Spaceship[0].Height / 2),
                                    1.2f,
                                    SpriteEffects.None,
                                    1.0f);

                    foreach (ObjetosADibujar obj in listaPelotas)
                    {
                        if (obj.MoB == 4)
                        {
                            spriteBatch.Draw(alien, new Rectangle(obj.PosicX - (int)alien.Width / 2, obj.PosicY - (int)alien.Height / 2 + 7, alien.Width, alien.Height - 7), Color.White);
                        }
                        else if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                    }

                    foreach (Bullets bullet in bullets)
                        bullet.Draw(spriteBatch);

                    spriteBatch.Draw(gas, new Rectangle(1, 55, 30, 30), tinta);
                    spriteBatch.Draw(heart, new Rectangle(1, 15, 30, 30), tinta);
                    spriteBatch.Draw(red, new Rectangle(38, 13, currentHealth, 34), tinta);
                    spriteBatch.Draw(orange, new Rectangle(38, 53, (int)currentGas, 34), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 10, 250, 40), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 50, 250, 40), tinta);
                    spriteBatch.DrawString(fuente, "Estrellas faltantes = " + estrellasFaltantes.ToString(),
                                        new Vector2(345, 15), Color.White);
                    break;
                #endregion
                case 5:
                    #region Draw Nivel 5
                    fondo1.Draw(spriteBatch);
                    fondo2.Draw(spriteBatch);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    spriteBatch.Draw(Spaceship[spaceshipActual],
                                    position,
                                    null,
                                    Color.White,
                                    rotation,
                                    new Vector2(
                                        Spaceship[0].Width / 2,
                                        Spaceship[0].Height / 2),
                                    1.2f,
                                    SpriteEffects.None,
                                    1.0f);
                    foreach (ObjetosADibujar obj in listaPelotas)
                        if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                        else if (obj.MoB == 1)
                        {
                            spriteBatch.Draw(bomba,
                                        obj.Posicion,
                                        null,
                                        Color.White,
                                        rotacionPiedra,
                                        new Vector2(bomba.Width / 2, bomba.Height / 2),
                                            1.0f,
                                            SpriteEffects.None,
                                            1.0f);
                        }
                        else if (obj.MoB == 2)
                        {
                            spriteBatch.Draw(gas, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }
                        else if (obj.MoB == 3)
                        {
                            spriteBatch.Draw(heart, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }

                    foreach (Bullets bullet in bullets)
                        bullet.Draw(spriteBatch);

                    spriteBatch.Draw(gas, new Rectangle(1, 55, 30, 30), tinta);
                    spriteBatch.Draw(heart, new Rectangle(1, 15, 30, 30), tinta);
                    spriteBatch.Draw(red, new Rectangle(38, 13, currentHealth, 34), tinta);
                    spriteBatch.Draw(orange, new Rectangle(38, 53, (int)currentGas, 34), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 10, 250, 40), tinta);
                    spriteBatch.Draw(bar, new Rectangle(35, 50, 250, 40), tinta);
                    if (aparecerTierra)
                        spriteBatch.Draw(tierra, new Rectangle(100, 0, tierra.Width, tierra.Height), Color.White);
                    else
                        spriteBatch.DrawString(fuente, "Estrellas faltantes = " + estrellasFaltantes.ToString(),
                                        new Vector2(345, 15), Color.White);
                    break;
                    #endregion
                case 6:
                    #region Draw Game Over
                    fondoGameOver.Draw(spriteBatch);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    if (!hoverAtras)
                        spriteBatch.Draw(botonVolver, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonVolverH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    if (!hoverFin)
                        spriteBatch.Draw(botonFin, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonFinH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                #endregion
                case 7:
                    #region Draw Creditos
                    fondoCreditos.Draw(spriteBatch);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));

                    if (!hoverAtras)
                        spriteBatch.Draw(botonAtras, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonAtrasH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                #endregion
                case 8:
                    #region Draw Ganador
                    fondoNivel5.Draw(spriteBatch);
                    if (!hoverAtras)
                        spriteBatch.Draw(botonVolver, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonVolverH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    if (!hoverFin)
                        spriteBatch.Draw(botonFin, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonFinH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                #endregion
                case 10:
                    fondoTemp.Draw(spriteBatch, new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    break;
                case 11:
                    fondoInstrucciones1.Draw(spriteBatch);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    if (!hoverSiguiente)
                        spriteBatch.Draw(botonSiguiente, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonSiguienteH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    if (!hoverAtras)
                        spriteBatch.Draw(botonAtras, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonAtrasH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                case 12:
                    fondoInstrucciones2.Draw(spriteBatch);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    if (!hoverSiguiente)
                        spriteBatch.Draw(botonSiguiente, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonSiguienteH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    if (!hoverAtras)
                        spriteBatch.Draw(botonAtras, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonAtrasH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                case 13:
                    fondoInstrucciones3.Draw(spriteBatch);
                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    if (!hoverSiguiente)
                        spriteBatch.Draw(botonSiguiente, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonSiguienteH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    if (!hoverAtras)
                        spriteBatch.Draw(botonAtras, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonAtrasH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);

                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
                case 14:
                    if (seleccion == 1)
                        fondoInstrucciones4Piedra.Draw(spriteBatch);
                    else if (seleccion == 2)
                        fondoInstrucciones4Estrella.Draw(spriteBatch);
                    else if (seleccion == 3)
                        fondoInstrucciones4Gas.Draw(spriteBatch);
                    else if (seleccion == 4)
                        fondoInstrucciones4Vida.Draw(spriteBatch);
                    else
                        fondoInstrucciones4.Draw(spriteBatch);

                    spriteBatch.Draw(logo, new Rectangle(300, 5, logo.Width, logo.Height), new Color((int)transparencia, (int)transparencia, (int)transparencia));
                    foreach (ObjetosADibujar obj in listaPelotas)
                        if (obj.MoB == 0)
                        {
                            spriteBatch.Draw(estrella,
                                            obj.Posicion,
                                            null,
                                            Color.White,
                                            rotacionEstrella,
                                            new Vector2(estrella.Width / 2, estrella.Height / 2),
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f);
                        }
                        else if (obj.MoB == 1)
                        {
                            spriteBatch.Draw(bomba,
                                        obj.Posicion,
                                        null,
                                        Color.White,
                                        rotacionPiedra,
                                        new Vector2(bomba.Width / 2, bomba.Height / 2),
                                            1.0f,
                                            SpriteEffects.None,
                                            1.0f);
                        }
                        else if (obj.MoB == 2)
                        {
                            spriteBatch.Draw(gas, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }
                        else if (obj.MoB == 3)
                        {
                            spriteBatch.Draw(heart, new Rectangle(obj.PosicX, obj.PosicY, 40, 40), Color.White);
                        }

                    if (!hoverFin)
                        spriteBatch.Draw(botonFin, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);
                    else
                        spriteBatch.Draw(botonFinH, new Rectangle(680, 482, botonSiguiente.Width, botonSiguiente.Height), Color.White);

                    if (!hoverAtras)
                        spriteBatch.Draw(botonAtras, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    else
                        spriteBatch.Draw(botonAtrasH, new Rectangle(12, 482, botonAtras.Width, botonAtras.Height), Color.White);
                    
                    spriteBatch.Draw(cursor, mousePosition, Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}