import math
import numpy as np
import matplotlib.pyplot as plt

class Grue :
    def __init__(self, m_platforme, m_grue, m_grappin, L_plateforme, V_flotteur ,h_plateforme, h_base, L_base, D=3):
        self.m_plateforme = m_platforme       #masse de la plateforme flottante [kg]
        self.m_grue = m_grue                  #masse de la base + 1er bras [kg]
        self.m_grappin = m_grappin            #masse du grappin + bras de grue attaché à celui-ci
        self.v_flotteur = V_flotteur          #volume desflotteurs utilisés
        self.D = D                            #coeficient d'amortissement [kg*m/s]
        
        self.m_totale = self.m_plateforme + self.m_grue +self.m_grappin
        #self.m_totale_nocharge = self.m_plateforme + self.m_grue + self.m_grappin+self.

        self.L = L_plateforme                            #longueur d'un coté de la plateforme [m]
        self.h_plateforme = h_plateforme                 #hauteur de la plateforme
        self.h_base = h_base                             #longueur d'un coté de la base [m]
        self.L_base = L_base                             #hauteur de la base

    def __str__(self):
        return("  --- {0} --- \n Masse plateforme :              {1} [kg] \n Masse de la base de la grue :   {2} [kg] \n Masse du grappin :              {3} [kg] \n Volume des flotteurs :          {4} [m^3] \n Longueur de la plateforme :     {5} [m] \n Hauteur de la plateforme :      {6} [m] \n Longueur de la base :           {7} [m] \n Hauteur de la base :            {8} [m] \n Hauteur immergée :             {9:.3f} [m]".format(self.m_totale,self.m_plateforme,self.m_grue,self.m_grappin,self.v_flotteur,self.L,self.h_plateforme,self.L_base,self.h_base,self.h_immergé(self.m_totale)))


    def h_immergé(self,m):
        """ pre : m, la masse de la grue + potentielle charge
            post : la hauteur immergée en [m]
        """
        return (m*self.h_plateforme)/(997*(self.v_flotteur))

    def angle_max (self,m_charge) :
        """ pre : m_charge, la masse de la charge
            post : l'angle maximum avant le renversement de la plateforme [rad]
        """
        return math.atan((self.h_plateforme-self.h_immergé(m_charge+self.m_totale))/(self.L/2))
    
    def theta_0(self, d,m_charge):
        """ pre : d, distance entre la charge te la base et m_charge, la masse de la charge
            post : l'angle au départ (avant de lacher la masse) de la plateforme [rad]
        """
        return math.atan((12*(m_charge+self.m_grappin)*d*self.h_immergé(self.m_totale+m_charge))/((self.m_totale+m_charge)*(self.L**2)))
    
    def inertie(self, d, h, m_charge):
        """ pre : d, distance entre la charge te la base, h, la hauteur de la charge par rappor à la platforme et m_charge, la masse de la charge
            post : l'inertie du système complet [kg*m^2]
        """
        i_plateforme = (1/12)*self.m_plateforme*(2*(self.L**2)) + self.m_plateforme*(((self.h_immergé(self.m_totale+m_charge)-(self.h_plateforme/2))**2)+(self.L/2)**2)
        i_base = (1/12)*self.m_grue*((self.L_base**2)+(self.h_base**2)) + self.m_grue*((self.h_plateforme-self.h_immergé(self.m_totale+m_charge)+(self.h_base/2)**2)+(0**2))
        return ((self.m_grappin + m_charge)*((d**2)+(h**2)) + i_plateforme + i_base)
    
    def angle_soulèvement (self, masse):
        """ pre : masse, la masse totale (grue+potentielle charge)
            post : l'angle avant le soulèvement de la plateforme [rad]
        """
        return math.atan((self.h_immergé(masse))/(self.L/2))
    
    def oscillation_lacher(self,d,h,m_charge,end,step) :
        """
        simulation : simulation de l'angle d'inclinaison en fonction du temps lors du 
            lacher d'une charge 

        pre:    d : la distance entre la grue et la charge lachée [m]
                h : la hauteur entre l'eau et la charge lachée [m]
                m_charges:masse de la charge lachée [kg]
                end : la durée du temps simulé [s]
                step : la durée d'une étape de la simulation (plus petit = plus précis) [s]
        post: exécute une simulation jusqu'à t=end par pas de dt=step.
            Remplit les listes x, v, a des positions, vitesses et accélérations.
        """

        # conditions initiales
        t = np.arange(0, end, step )                               #temps  [s]
        theta = np.empty_like(t)                                   #angle [rad]
        w = np.empty_like(t)                                       #vitesse angulaire (dtheta/dt) [rad/s]
        a_angle = np.empty_like(t)                                 #acceleration angulaire (dw/dt) [rad/s**2]
        e_cinétique= np.empty_like(t)                              #énergie cinétique [J]
        e_pot= np.empty_like(t)                                    #énergie des flotteurs [J]
        e_tot= np.empty_like(t)                                    #énergie totale [J]
        angle_max = self.angle_max(0)
        angle_soulèvement=self.angle_soulèvement(self.m_totale)
        h_immergé = (self.h_immergé(self.m_totale))
        m_totale = (self.m_totale)
        L = (self.L)
        m_grappin = (self.m_grappin)
        
        theta[0] = self.theta_0(d,m_charge)
        w[0] = 0
        
        for i in range(len(t)-1):

            dt = step
            
            # calcul de la force totale
            X_G = ((m_grappin)*d)/m_totale                               #position centre de gravité [m]
            X_r = (((L**2)*math.tan(theta[i]))/(12*h_immergé))         #position centre de poussé [m]

            C_r = m_totale*9.81*(X_G-X_r)                              #couple de redressement [Nm]

            C_D = -self.D*w[i]                                         #couple déstabiloisateur  [Nm]                            

            C_total = C_r + C_D                                        #couple total [Nm]
            
            # calcul accélération, vitesse, position
            a_angle[i] = C_total / self.inertie(d,h,m_totale)
            w[i+1] = w[i] + a_angle[i] * dt
            theta[i+1] = theta[i] + w[i] * dt
            #a_angle[i+1] = a_angle[i]
            
            #calcul des énergies
            e_cinétique[i]=((1/2)*(w[i]**2)*(self.m_totale*((self.L/2)**2)))
        p=1
        n=False
        while n==False:
            if a_angle[p]>0:
                n=True
            else :
                p+=1
        
        for i in range (len(t)-p):
            e_pot[i]=e_cinétique[i+p]
        for i in range (len(t)):
            e_tot[i]=e_pot[i]+e_cinétique[i]

            #afficher le graphique 

                
        plt.figure("oscillation lacher")
        plt.plot(t,(theta*180/math.pi), label="θ")
        plt.title("Oscillation causé par le lacher d'une masse")
        plt.xlabel("temps [s]")
        plt.ylabel("angle [°]")
        plt.plot([t[0],t[-1]], [(angle_max*180/math.pi), (angle_max*180/math.pi)], '--r',label = "angle max")
        plt.plot([t[0],t[-1]], [-(angle_max*180/math.pi), -(angle_max*180/math.pi)], '--r')
        plt.plot([t[0],t[-1]], [(angle_soulèvement*180/math.pi), (angle_soulèvement*180/math.pi)], '--b',label = "angle soulèvement")
        plt.plot([t[0],t[-1]], [-(angle_soulèvement*180/math.pi), -(angle_soulèvement*180/math.pi)], '--b')
        plt.legend()
        plt.show()
        
        plt.figure("oscillation lacher vitesse")
        plt.title("Oscillation causé par le lacher d'une masse: vitesse angulaire")
        plt.subplot(1,1,1)
        plt.plot(t,(w*180/math.pi), label="ω")
        plt.xlabel("temps [s]")
        plt.ylabel("vitesse angulaire [°/s]")
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation lacher accélération")
        plt.title("Oscillation causé par le lacher d'une masse: accélération angulaire")
        plt.subplot(1,1,1)
        plt.plot(t,(a_angle*180/math.pi), label="ω'")
        plt.xlabel("temps [s]")
        plt.ylabel("accéleration angulaire [°/s^2]")
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation lacher énergie")
        plt.title("Oscillation causé par le lacher d'une masse: bilan des énergies")
        plt.subplot(1,1,1)
        plt.plot(t,(e_cinétique), label="énergie_cinétique")
        plt.xlabel("temps [s]")
        plt.ylabel("énergie [J]")
        plt.legend()
        
        plt.subplot(1,1,1)
        plt.plot(t,(e_pot), label="énergie_des_flotteurs")
        plt.xlabel("temps [s]")
        plt.ylabel("énergie [J]")
        plt.legend()
        
        plt.subplot(1,1,1)
        plt.plot(t,(e_tot), label="énergie_totale")
        plt.xlabel("temps [s]")
        plt.ylabel("énergie [J]")
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation lacher diagramme de phase")
        plt.title("Diagramme de phase")
        plt.plot(theta*180/math.pi, a_angle*180/math.pi,label="Diagramme de phase")
        plt.xlabel("theta (º)")
        plt.ylabel("vitesse angulaire (º/s)")
        plt.legend()
        
        plt.show()
    
    def oscillation_mouvement(self,t_deplacement,h,d_max,m_charge,end,step) :
        """ 
            simulation : simulation de l'angle d'inclinaison en fonction du temps lors du 
            déplacement d'une charge sur une distance d + l'oscillation après la fin du déplacement

            pre:    h : hauteur à laquelle se passe le déplacement
                    t_deplacement : temps du deplacement [s]
                    d_max : distance max du déplacement [m]
                    m_charge : masse de la charge [kg]
                    step : temps d'une intervale (plus petit = plus précis) [s]
                    end : temps de fin de simu [s]
            
            post:   affiche un graphique de l'angle en fonction du temps   
        """

        # conditions initiales
        t = np.arange(0, end, step )                               #temps  [s]
        theta = np.empty_like(t)                                   #angle [rad]
        w = np.empty_like(t)                                       #vitesse angulaire (dtheta/dt) [rad/s]
        a_angle = np.empty_like(t)                                 #acceleration angulaire (dw/dt) [rad/s**2]
        t_deplacement=t_deplacement
        d = np.empty_like(t)                                       #distance [d]
        v = d_max/t_deplacement                                    #vitesse de déplacement de la charge
        e_cinétique= np.empty_like(t)                              #énergie cinétique [J]
        e_pot= np.empty_like(t)                                    #énergie des flotteurs [J]
        e_tot= np.empty_like(t)                                    #énergie totale [J]
        angle_max = self.angle_max(0)
        m_totale = (self.m_totale+m_charge)
        angle_soulèvement=self.angle_soulèvement(self.m_totale)
        h_immergé = (self.h_immergé(self.m_totale))
        L = (self.L)
        m_grappin = (self.m_grappin)
        
        d[0] = 0
        theta[0] = self.theta_0(d[0],m_charge)  #angle initial [rad]
        w[0] = 0                                #vitesse angulaire initiale [rad/s] 
        
        dt=step
        
        for i in range(len(t)-1) :

            if d[i] < d_max :
                d[i+1] = d[i] + v*dt
            else: 
                d[i+1] = d_max

            # calcul de la force totale
            X_r = (((L**2)*math.tan(theta[i]))/(12*h_immergé))          #position centre de poussée
            X_G = (((m_charge+m_grappin)*d[i]))/m_totale                              #position centre de gravité

            C_r = (m_totale+m_charge)*9.81*(X_G-X_r)                               #couple de redressement

            C_D = -self.D*w[i]                                

            C_total = C_r + C_D

            # calcul accélération, vitesse, position
            a_angle[i] = C_total / self.inertie(d[i],h,m_totale)
            w[i+1] = w[i] + a_angle[i] * dt
            theta[i+1] = theta[i] + w[i] * dt
            a_angle[i+1] = a_angle[i]
            
            #calcul des énergies
            e_cinétique[i]=((1/2)*(w[i]**2)*(self.m_totale*((self.L/2)**2)))
        p=0
        r=0
        c=0
        last_p=0
        for i in range (len(t)):
            if d[i]<d_max:
                if i>=r:
                    last_p=p
                    r+=p
                    p=r
                    c+=1
                    n=False
                    while n==False:
                        if a_angle[p]*((-1)**c)<0:
                            n=True
                        else:
                            p+=1
                    p=p-r
                    if d[i+p]==d_max:
                        p=last_p
                try:
                    if p!=0 and p!=1:
                        e_pot[i]=e_cinétique[i+p]  
                except IndexError:
                    pass
            else:
                if i==int(t_deplacement//dt)+1:
                    p=int(t_deplacement//dt)+1
                    n=False
                    while n==False:
                        if a_angle[p+1]-a_angle[p]>0:
                            n=True
                        else:
                            p+=1
                    p=p-int(t_deplacement//dt)
                try:
                    e_pot[i-p]=e_cinétique[i]
                except IndexError:
                    pass
            e_pot[0]=e_pot[1]
        for i in range(len(t)):
            e_tot[i]=e_pot[i]+e_cinétique[i]

            #afficher le graphique 

                
        plt.figure("oscillation mouvement")
        plt.plot(t,(theta*180/math.pi), label="θ")
        plt.title("Oscillation causé par le mouvenment d'une masse")
        plt.xlabel("temps [s]")
        plt.ylabel("angle [°]")
        plt.plot([t[0],t[-1]], [(angle_max*180/math.pi), (angle_max*180/math.pi)], '--r',label = "angle max")
        plt.plot([t[0],t[-1]], [-(angle_max*180/math.pi), -(angle_max*180/math.pi)], '--r')
        plt.plot([t[0],t[-1]], [(angle_soulèvement*180/math.pi), (angle_soulèvement*180/math.pi)], '--b',label = "angle soulèvement")
        plt.plot([t[0],t[-1]], [-(angle_soulèvement*180/math.pi), -(angle_soulèvement*180/math.pi)], '--b')
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation mouvement vitesse")
        plt.title("Oscillation causé par le mouvement d'une masse: vitesse angulaire")
        plt.subplot(1,1,1)
        plt.plot(t,(w*180/math.pi), label="ω")
        plt.xlabel("temps [s]")
        plt.ylabel("vitesse angulaire [°/s]")
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation mouvement accélération")
        plt.title("Oscillation causé par le mouvement d'une masse: accélération angulaire")
        plt.subplot(1,1,1)
        plt.plot(t,(a_angle*180/math.pi), label="ω'")
        plt.xlabel("temps [s]")
        plt.ylabel("accéleration angulaire [°/s^2]")
        plt.legend()
        
        plt.show()
        
        plt.figure("oscillation mouvement énergie")
        plt.title("Oscillation causé par le mouvement d'une masse: bilan des énergies")
        plt.subplot(1,1,1)
        plt.plot(t,(e_cinétique), label="énergie_cinétique")
        plt.xlabel("temps [s]")
        plt.ylabel("énergie [J]")
        plt.legend()
        
        plt.subplot(1,1,1)
        plt.plot(t,(e_pot), label="énergie_des_flotteurs")
        plt.legend()
        
        plt.subplot(1,1,1)
        plt.plot(t,(e_tot), label="énergie_totale")
        plt.legend()
        
        plt.show()
                
        plt.figure("oscillation mouvement diagramme de phase")        
        plt.title("Diagramme de phase")
        plt.plot(theta*180/math.pi, a_angle*180/math.pi,label="Diagramme de phase")
        plt.xlabel("theta (º)")
        plt.ylabel("vitesse angulaire (º/s)")
        plt.legend()
        
        plt.show()
    
    def angle_par_d(self,d_max,masses,step) :          
        """
        simulation : simulation de l'angle d'inclianaison en fontion de d pour une masse transportée donnée

            pre:    d_max : distance maximum de la simulation [m]
                masses : une liste de masse des différentes charges [kg]
                step : distance entre chaque etape de la simulation (plus petit = plus précis) [s]
            
            post:   affiche un graphique graphique de l'angle d'inclianaison en fontion de d pour une masse transportée donnée
        """
        
        plt.figure("Angle en fonction du déplacement de masses")
        plt.title("Angle en fonction du déplacement de masses")
        plt.xlabel("distance [m]")
        plt.ylabel("angle [°]")

        for i in range(len(masses)):
            d = np.arange(0,d_max,step)
            angle = np.arange(0,d_max,step)
            angle_max = self.angle_max(masses[i])
            angle_soulèvement = self.angle_soulèvement(self.m_totale+masses[i])
            
            for n in range(len(d)) :
                angle[n]= self.theta_0(d= d[n],m_charge= masses[i])
                
            plt.plot(d,(angle*180/math.pi),label ="θ : " + str(masses[i]) +" kg")

        plt.plot([d[0],d[-1]], [(angle_max*180/math.pi), (angle_max*180/math.pi)], '--r', label='submersion')
        plt.plot([d[0],d[-1]], [(angle_soulèvement*180/math.pi), (angle_soulèvement*180/math.pi)], '--b', label='soulèvement')
        plt.legend()
        
        plt.show()