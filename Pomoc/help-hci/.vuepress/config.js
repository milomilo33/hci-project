module.exports = {
  title: 'Pomoć - Organizator',
  description: 'Online pomoć za korišćenje wpf aplikacije Organizator',
  head: [
    ['meta', { name: 'theme-color', content: '#3eaf7c' }],
    ['meta', { name: 'apple-mobile-web-app-capable', content: 'yes' }],
    ['meta', { name: 'apple-mobile-web-app-status-bar-style', content: 'black' }],
    ['meta', { httpEquiv: 'X-UA-Compatible', content:'IE=edge'}]
  ],
  themeConfig: {
    repo: '',
    editLinks: false,
    docsDir: '',
    editLinkText: '',
    lastUpdated: false,
    nav: [
      { 
        text: 'Klijent', 
        link: '/klijent/',
      },
      {
        text: 'Administrator', 
        link: '/administrator/'
      },
      { 
        text: 'Organizator', 
        link: '/organizator/'
      }
    ],
    sidebar: {
      '/klijent/': [
        {
          title: 'Klijent - korisnička pomoć',
          collapsable: false,
          children: [
            'prijava',
            'odjava',
            'registracija',
            'profil',
            'pocetna-stranica',
            'pregled-dogadjaja',
            'pregled-organizatora',
            'kreiranje-dogadjaja',
          ]
        }
      ],
      '/administrator/': [
        {
          title: 'Administrator - korisnička pomoć',
          collapsable: false,
          children: [
            'prijava',
            'odjava',
            'profil',
            'pocetna-stranica',
            'pregled-organizatora',
            'pregled-klijenata',
            'pregled-dogadjaja',
          ]
        }
      ],
      '/organizator/': [
        {
          title: 'Organizator - korisnička pomoć',
          collapsable:  false,
          children: [
            'prijava',
            'odjava',
            'profil',
            'pocetna-stranica',
            'pregled-saradnika',
            'pregled-dogadjaja',
            'pregled-ponuda',
            'dodeljeni-dogadjaji'
          ]
        }
      ]
    }
  },
  plugins: [
    '@vuepress/plugin-back-to-top',
    '@vuepress/plugin-medium-zoom',
  ]
}