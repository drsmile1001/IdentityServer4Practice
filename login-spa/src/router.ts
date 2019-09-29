import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import("./views/Home.vue")
    },
    {
      path: '/callback',
      component: () => import('./views/CallBack.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: ()=>import("@/views/Login.vue")
    },
    {
      path: '/error',
      name: 'error',
      component: ()=>import("@/views/Error.vue")
    },
    {
      path: '/logout',
      name: 'logout',
      component: ()=>import("@/views/Logout.vue")
    },
  ]
})
