
exports.seed = function(knex) {
  // Deletes ALL existing entries
  return knex('imtb_account').del()
    .then(function () {
      // Inserts seed entries
      return knex('imtb_account').insert([
        {License: '0501d892d0658910dad4cc121f22bc84e110afd9', Username: 'Admin', Password: '123456', CreatedAt: '2021-06-30 17:32:37', UpdatedAt: '2021-06-30 17:32:37', AdminLevel: 3001, DonateRank: 2, Level: 15, Respect: 1, ConnectedTime: 13, Money: 1000, Bank: 2000, PedModel: 'a_m_m_og_boss_01', },
      ]);
    });
};
