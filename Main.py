from simulation_Sjöaveld import *

"""
    fichier principal pour créer des grues et les simulations qui vont avec

    créer une grue (grue type) :
    
        ma_grue = Grue(m_platforme= ,m_grue= ,m_grappin= , L_plateforme= , V_flotteur= ,h_plateforme= ,h_base= ,L_base=,D=)

                m_plateforme : masse de la plateforme [kg]
                m_grue : masse de la base de la grue (partie non mobile + premier bras) [kg]
                m_grappin : masse du grappin + le 2eme bras de la grue [kg]
                L_plateforme : longueur d'un coté de la plateforme (plateforme carrée) [m]
                V_flotteur : volume total des flotteurs [m**3]
                h_base : estimation de la hauteur de la base de la grue [m]
                L_base : estimation de la longueur d'un coté de la base [m]
                D: coefficient d'amortissement [kg*m/s] (3 par défault)

    simulation possible :
    
        oscillation_mouvement() : montre l'angle d'inclinaison de la plateforme en fonction du temps avec une 
                                  masse qui se deplace jusqu'à une distance d_max en un temps t_dplacement

                ma_grue.oscillation_mouvement(h= ,t_deplacement= ,d_max= ,m_charge= ,step= ,end= )

                h : hauteur à laquelle est la masse transportée [m]
                t_deplacement : temps que prends la masse à être deplacée [s]
                d_max : distance à laquelle la masse va être transportée [m]
                m_charge : masse de la charge transportée [kg]
                step : temps d'une étape de la simulation (plus petit = plus précis) [s]
                end : temps de la fin de la simulation [s]
                
        oscillation_lacher() : montre l'angle,la vitesse angualaire et l'acceleration angulaire 
                               lorsque une masse est lachée à une certaine distance et hauteur

                ma_grue.oscillation_lacher(d = ,h = ,m_charges= ,end = ,step =)

                d : la distance entre la grue et la charge lachée [m]
                h : la hauteur entre l'eau et la charge lachée [m]
                m_charges:masse de la charge lachée [kg]
                end : la durée du temps simulé [s]
                step : la durée d'une étape de la simulation (plus petit = plus précis) [s]

        angle_par_d() : montre l'angle stable de la grue en fontion de la distance entre
                        la grue et la charge pour différentes masses de charge
                
                ma_grue.angle_par_d( d_max = ,masses = step = )

                d_max : distance maximum de la simulation [m]
                masses : une liste de masse des différentes charges [kg]
                step : distance entre chaque etape de la simulation (plus petit = plus précis) [s]

"""

Sjöaveld = Grue(m_platforme= 2.246 ,m_grue= 2.045 ,m_grappin= 0.540 , L_plateforme= 0.6, V_flotteur= 0.012 ,h_plateforme=0.08 ,h_base=0.4 ,L_base=0.2)
Sjöaveld.oscillation_mouvement(h=0.4,t_deplacement=3,d_max=0.7,m_charge=0.2,end=10,step=0.0001)
Sjöaveld.oscillation_lacher(d=0.7,h=0.4,m_charge=0.2,end=10,step=0.0001)
Sjöaveld.angle_par_d(d_max=0.7,masses=[0.05,0.1,0.15,0.2,0.25,0.3],step=0.0001)